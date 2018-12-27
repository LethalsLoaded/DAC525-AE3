using UnityEngine;

public class Level_7_Level_Manager : MonoBehaviour {

	void Start ()
	{
        CanvasScript.SetNotificationBoxTitle("Moving Platforms");
		CanvasScript.SetNotificationBoxText("I'm sure you know what needs to be done here...");
		CanvasScript.SetNotificationBoxCloseText("For real?");
		CanvasScript.ShowNotificationBox();

        InputManager.GetInstance()._onNotificationClose.AddListener(GameManager.PrepareLevel);
	}
	
	void Update ()
	{
		
	}
}
