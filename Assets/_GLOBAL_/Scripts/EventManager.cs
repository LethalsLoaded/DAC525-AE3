using System.Collections.Generic;
using System.Linq;
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

	public UnityEvent OnSwipe;
	public UnityEvent OnTap;
	[System.Serializable]
	public class OnHoldEvent : UnityEvent<Touch> {}
	public OnHoldEvent OnHold;


	
	// Use this for initialization
	void Start ()
	{
		if (instance == null) instance = this;
		else if (instance != this) Destroy(gameObject);		
	}
	
	public static EventManager GetInstance()
	{
		if(instance == null) throw new System.Exception("Event Manager hasn't been initialaized.");
		return instance;
	}

	public static SwipeDirection GetSwipeDirection(Touch swipe)
	{
		return SwipeDirection.UP;
	}

}
