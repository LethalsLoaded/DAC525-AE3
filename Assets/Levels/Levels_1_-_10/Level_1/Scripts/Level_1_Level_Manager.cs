﻿using UnityEngine;

public class Level_1_Level_Manager : MonoBehaviour
{

	void Start ()
	{
		CanvasScript.SetNotificationBoxTitle("Welcome!");
		CanvasScript.SetNotificationBoxText("In order to complete this level you need to kill all the target dummies.\n"
		+ "To attack press the 'ATTACK' button on bottom right.\n"
		+ "\n"
		+ "Once all the enemies are dead, proceed to the door!");
		CanvasScript.SetNotificationBoxCloseText("Roger that!");
		CanvasScript.ShowNotificationBox();
		
		InputManager.GetInstance()._onNotificationClose.AddListener(GameManager.PrepareLevel);
	}
	
	void Update ()
	{
		
	}
}
