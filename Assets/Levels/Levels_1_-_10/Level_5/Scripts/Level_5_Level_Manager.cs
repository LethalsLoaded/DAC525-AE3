using UnityEngine;
using System.Linq;

public class Level_5_Level_Manager : MonoBehaviour {

	void Start ()
	{
        CanvasScript.SetNotificationBoxTitle("Enemies");
		CanvasScript.SetNotificationBoxText("These guys can actually be real nasty.\n"
		+ "Due to their <i>Hostile Touch</i> ability I'd not get too close to them...\n"
		+ "\n"
		+ "Though, you have to kill them to do this level!");
		CanvasScript.SetNotificationBoxCloseText("Well then...");
		CanvasScript.ShowNotificationBox();
		
		InputManager.GetInstance()._onNotificationClose.AddListener(GameManager.PrepareLevel);
	}
	bool allTasksComplete = false;

	void Update ()
	{
        if (allTasksComplete) enabled = false;
        var aliveEnemies = GameObject.FindGameObjectsWithTag("ENTITY").Where(x=> !x.GetComponent<Entity>().isDead).Count();
        if(aliveEnemies != 0) return;
        allTasksComplete = true;
        GameObject.FindGameObjectWithTag("DOOR").GetComponent<DoorScript>()._isDoorOpen = true;
	}
}
