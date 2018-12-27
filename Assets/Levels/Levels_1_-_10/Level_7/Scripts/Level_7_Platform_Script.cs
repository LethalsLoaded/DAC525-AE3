using UnityEngine;

public class Level_7_Platform_Script : MonoBehaviour {

    public enum MovementType
    {
        ANIMATION,
        NODES,
        CIRCULAR
    }

    public MovementType platformMovementType;

    [Header("Animation")]
    public AnimationClip animationClip;

    [Header("- Nodes - ")]
    public Vector2 pointOne;
    public Vector2 pointTwo;
    public float nodalSpeed;

    [Header("- Circular -")]
    public Vector2 origin;
    public float rotationSpeed;

    private float _angle;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		switch(platformMovementType)
        {
            case MovementType.ANIMATION:
            PlayAnimation();
            break;
            case MovementType.CIRCULAR:
            MoveAroundPoint();
            break;
            case MovementType.NODES:
            MoveAroundNodes();
            break;
        }
	}

    void PlayAnimation()
    {
        var animator = gameObject.GetComponent<Animation>() ?? gameObject.AddComponent<Animation>();
        if(animator.isPlaying) return;
        animator.clip = animationClip;
        animator.Play();
    }


    bool _movingToPointTwo = true;

    void MoveAroundNodes()
    {
        var speed = nodalSpeed * Time.deltaTime;
        if(_movingToPointTwo)
            transform.position = Vector2.MoveTowards(transform.position, pointTwo, speed);
        else
            transform.position = Vector2.MoveTowards(transform.position, pointOne, speed);

        if(((Vector2)transform.position == pointTwo))
            _movingToPointTwo = false;
        else if (((Vector2)transform.position == pointOne))
            _movingToPointTwo = true;
    }

    void MoveAroundPoint()
    {
        transform.RotateAround(origin, new Vector3(0,0,1), rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.identity;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag != "PLAYER") return;
        collider.transform.parent = this.transform;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag != "PLAYER") return;
        collider.transform.parent = null;
    }
}
