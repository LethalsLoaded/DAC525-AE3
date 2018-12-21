/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent _onSwipeUpEvent, _onSwipeDownEvent, _onSwipeRightEvent, _onSwipeLeftEvent;
    private readonly IList<int> blacklist = new List<int>();
    public IList<int> touchesToTrack = new List<int>();


    private void Start()
    {
        // Ensure that we have MT enabled.
        Input.multiTouchEnabled = true;
    }

    private void Update()
    {
        // foreach(var i in Input.touches)
        // {
        //     if(i.phase == TouchPhase.Began)
        //         touchesToTrack.Add(i.fingerId);
        //     else if (i.phase == TouchPhase.Ended)
        //         touchesToTrack.Remove(i.fingerId);
        // }


        foreach (var i in Input.touches.Where(x =>
            x.phase != TouchPhase.Ended && x.GetSwipeDirection() != TouchHandler.directions.None))
        {
            if (blacklist.Contains(i.fingerId))
                break;
            Debug.Log($"A: {i.GetSwipeDirection()}");
            switch (i.GetSwipeDirection())
            {
                case TouchHandler.directions.Up:
                    _onSwipeUpEvent.Invoke();
                    break;
                case TouchHandler.directions.Down:
                    _onSwipeDownEvent.Invoke();
                    break;
                case TouchHandler.directions.Left:
                    _onSwipeLeftEvent.Invoke();
                    break;
                case TouchHandler.directions.Right:
                    _onSwipeRightEvent.Invoke();
                    break;
            }

            blacklist.Add(i.fingerId);
        }

        foreach (var i in Input.touches.Where(x => x.phase == TouchPhase.Ended))
            blacklist.Remove(i.fingerId);

        // foreach(var i in Input.touches.Where(x=> touchesToTrack.Contains(x.fingerId) && x.phase == TouchPhase.Ended))
        // {
        //     switch(i.GetSwipeDirection())
        //     {
        //         case TouchHandler.directions.Up:
        //         _onSwipeUpEvent.Invoke();
        //         break;
        //         case TouchHandler.directions.Down:
        //         _onSwipeDownEvent.Invoke();
        //         break;
        //         case TouchHandler.directions.Left:
        //         _onSwipeLeftEvent.Invoke();
        //         break;
        //         case TouchHandler.directions.Right:
        //         _onSwipeRightEvent.Invoke();
        //         break;
        //     }
        // }
    }
}