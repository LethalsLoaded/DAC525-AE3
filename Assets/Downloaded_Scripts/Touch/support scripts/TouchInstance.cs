using UnityEngine;

/* 
*** TOUCH INSTANCE CLASS ***
Property container for each touch object
 */

public class TouchInstance
{
    public Touch _touch;

    public TouchHandler.actions action;
    public Vector2 currentPos;
    public float currentPressTime; // press time of current tap
    public float distanceTraveled; // distance in drag units
    public int fingerId;

    public bool hasMoved;
    public float magnitude;
    public TouchHandler.actions overrideAction;
    public TouchPhase phase;
    public RaycastHit raycastHit;
    public RaycastHit2D raycastHit2D;

    private bool raycastOverride;
    private bool raycastOverride2D;
    public float speed;
    public Vector2 startPos;
    public float startTime;
    public TouchHandler.directions swipeDirection;
    private Vector2 swipeStartPos;
    public int tapCount;

    private float tapTimer;
    public float totalPressTime; // sum of press time over multiple taps
    public Vector2 velocity;

    // INITIALIZE NEW INSTANCE
    public TouchInstance()
    {
    }

    public TouchInstance(Touch t)
    {
        _touch = t;
        fingerId = t.fingerId;
        startPos = _touch.position;
        swipeStartPos = _touch.position;
        startTime = Time.time;
        action = TouchHandler.actions.Down;
        overrideAction = TouchHandler.actions.None;
    }

    // SWIPE DIRECTION
    private TouchHandler.directions GetSwipeDirection()
    {
        var direction = _touch.position - swipeStartPos;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0)
                return TouchHandler.directions.Left;
            return TouchHandler.directions.Right;
        }

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y < 0)
                return TouchHandler.directions.Down;
            return TouchHandler.directions.Up;
        }

        return TouchHandler.directions.None;
    }

    // SET ACTION
    private void SetAction()
    {
        if (hasMoved)
        {
            // SWIPE
            if (speed > TouchHandler._swipeThreshold)
            {
                action = TouchHandler.actions.Swipe;
                swipeDirection = GetSwipeDirection();
            }
            else
            {
                swipeStartPos = currentPos;
            }

            // DRAG
            if (action != TouchHandler.actions.Swipe) action = TouchHandler.actions.Drag;
        }
        else
        {
            // TAP
            if (_touch.phase.IsDone() &&
                currentPressTime < TouchHandler._longPressTime)
                action = TouchHandler.actions.Tap;
            // LONG-PRESS
            if (currentPressTime > TouchHandler._longPressTime) action = TouchHandler.actions.LongPress;
        }
    }

    // JUST (RE)TAPPED
    public void AddTap()
    {
        tapCount++;
        tapTimer = 0f;
        currentPressTime = 0f;
        distanceTraveled = 0f;

        startPos = _touch.position;
        swipeStartPos = _touch.position;
        overrideAction = TouchHandler.actions.None;
        hasMoved = false;
    }


    // RETURN CORRECT TOUCH
    public Touch GetTouchById(int fingerId)
    {
        foreach (var t in TouchHandler.touches)
            if (t.fingerId == fingerId)
                return t;
        return _touch;
    }

    // CHECK IF USER IS TOUCHING SOMETHING 3D
    public void CheckRayHit(int[] layerMask)
    {
        var ray = Camera.main.ScreenPointToRay(currentPos);
        if (raycastOverride == false)
        {
            var finalMask = 0;
            foreach (var layerNumber in layerMask) finalMask |= 1 << layerNumber;
            Physics.Raycast(ray, out raycastHit, Mathf.Infinity, ~finalMask);
        }
        else
        {
            raycastHit.point =
                ClosestPointOnRay(ray.origin, ray.GetPoint(1000f), raycastHit.rigidbody.transform.position);
        }
    }

    // CHECK IF USER IS TOUCHING SOMETHING 2D
    public void CheckRayHit2D(int[] layerMask)
    {
        Vector2 ray = Camera.main.ScreenToWorldPoint(currentPos);
        if (raycastOverride2D == false)
        {
            var finalMask = 0;
            foreach (var layerNumber in layerMask) finalMask |= 1 << layerNumber;
            raycastHit2D = Physics2D.Raycast(ray, Vector2.zero);
        }
        else
        {
            raycastHit2D.point = ray;
        }
    }

    public void GrappleRayHit()
    {
        if (raycastHit.collider != null)
            raycastOverride = true;
        if (raycastHit2D.collider != null)
            raycastOverride2D = true;
    }

    public void UngrappleRayHit()
    {
        raycastOverride = false;
        raycastOverride2D = false;
    }

    /// <summary>
    ///     Get wold point on ray that is closes to objPosition
    /// </summary>
    /// <returns>Vector3 World Point.</returns>
    /// <param name="rayOrigin">Ray origin.</param>
    /// <param name="rayEnd">Arbitrary end point of ray (needs to be further from origin than objPosition)</param>
    /// <param name="objPosition">Object position.</param>
    public Vector3 ClosestPointOnRay(Vector3 rayOrigin, Vector3 rayEnd, Vector3 objPosition)
    {
        var vVector1 = objPosition - rayOrigin;
        var vVector2 = (rayEnd - rayOrigin).normalized;

        var d = Vector3.Distance(rayOrigin, rayEnd);
        var t = Vector3.Dot(vVector2, vVector1);

        if (t <= 0)
            return rayOrigin;

        if (t >= d)
            return rayEnd;

        var vVector3 = vVector2 * t;

        var vClosestPoint = rayOrigin + vVector3;

        return vClosestPoint;
    }

    // UPDATE CALLED BY TouchHandler
    public void Update()
    {
        // refresh _touch
        _touch = GetTouchById(fingerId);
        phase = _touch.phase;

        // add a new tap
        if (phase == TouchPhase.Began) AddTap();

        // update properties
        currentPos = _touch.position;
        magnitude = _touch.deltaPosition.magnitude;
        speed = magnitude.PixelsToTouchUnits() / _touch.deltaTime;
        velocity = _touch.deltaPosition;


        // Moved > dragThreshold
        if (Vector2.Distance(startPos, currentPos) > TouchHandler._dragThreshold)
        {
            hasMoved = true;
            distanceTraveled += magnitude / TouchHandler._dragThreshold;
        }

        // start checking for timeout
        // if touch is done
        if (_touch.phase.IsDone())
        {
            tapTimer += Time.deltaTime;
            if (tapTimer > TouchHandler._tapTimeout) TouchHandler._touchCache.Remove(this);
        }
        else
        {
            totalPressTime += Time.deltaTime;
            currentPressTime += Time.deltaTime;
        }

        // ACTIONS
        if (overrideAction != TouchHandler.actions.None)
            action = overrideAction;
        else
            SetAction();
    }
}