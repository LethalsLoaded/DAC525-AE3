using UnityEngine;

public class Level_6_Level_Manager : MonoBehaviour {

	void Start ()
	{
        CanvasScript.SetNotificationBoxTitle("Platforms");
		CanvasScript.SetNotificationBoxText("You will learn to hate platforms by the time you finish this game.\n"
		+ "In this level you are to across the level. Simple, right?\n"
		+ "\n"
		+ "Oh, by the way, don't stand on the ones that are on fire!");
		CanvasScript.SetNotificationBoxCloseText("On fire?");
		CanvasScript.ShowNotificationBox();

        InputManager.GetInstance()._onNotificationClose.AddListener(GameManager.PrepareLevel);
	}
	
	void Update ()
	{
		
	}
}
