using UnityEngine;

public class Level_4_Level_Manager : MonoBehaviour
{

	public GameObject levelDoor;

	void Start ()
	{
		CanvasScript.SetNotificationBoxTitle("Locks and keys");
		CanvasScript.SetNotificationBoxText("There appears to be a door in the way which won't let you progress..\n"
		+ "There for sure has to be a key left over somewhere.\n"
		+ "\n"
		+ "<i>I mean, the bad guys always leave keys behind.. right?</i>");
		CanvasScript.SetNotificationBoxCloseText("Like a ninja!");
		CanvasScript.ShowNotificationBox();

		InputManager.GetInstance()._onNotificationClose.AddListener(GameManager.PrepareLevel);
	}
	
	void Update ()
	{
		
	}
}
