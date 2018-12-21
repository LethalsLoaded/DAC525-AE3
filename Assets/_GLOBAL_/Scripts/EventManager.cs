using System;
using UnityEngine;
using UnityEngine.Events;

public enum SwipeDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public UnityEvent OnClick;
    public OnHoldEvent OnHold;

    public UnityEvent OnSwipe;
    public UnityEvent OnTap;


    // Use this for initialization
    private void Start()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public static EventManager GetInstance()
    {
        if (instance == null) throw new Exception("Event Manager hasn't been initialaized.");
        return instance;
    }

    public static SwipeDirection GetSwipeDirection(Touch swipe)
    {
        return SwipeDirection.UP;
    }

    [Serializable]
    public class OnHoldEvent : UnityEvent<Touch>
    {
    }
}