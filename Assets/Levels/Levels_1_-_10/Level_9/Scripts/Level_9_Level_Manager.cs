using UnityEngine;

public class Level_9_Level_Manager : MonoBehaviour {

	void Start ()
	{
        CanvasScript.SetNotificationBoxTitle("Bouncy!");
		CanvasScript.SetNotificationBoxText("This level is designed to teach you how to bounce!\n"
		+ "Simply, jump ontop of the spring and <i>JUMP</i>\n"
		+ "\n"
		+ "Get to the door, and continue your adventure!");
		CanvasScript.SetNotificationBoxCloseText("Waheeey!");
		CanvasScript.ShowNotificationBox();

        InputManager.GetInstance()._onNotificationClose.AddListener(GameManager.PrepareLevel);
	}
	
	void Update ()
	{
		
	}
}
