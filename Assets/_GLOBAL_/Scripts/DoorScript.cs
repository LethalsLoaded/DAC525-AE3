/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool _isDoorOpen;

    public void ToggleDoorState()
    {
        _isDoorOpen = !_isDoorOpen;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!_isDoorOpen) return; //Print message saying do X to open door?
        GameLevel.NextLevel();
    }
}