using UnityEngine;

public class Level_8_Level_Manager : MonoBehaviour
{

	public Ability _newAbility;

	void Start ()
	{
		CanvasScript.SetNotificationBoxTitle("Locks and keys");
		CanvasScript.SetNotificationBoxText("Looks the same as an older room... where is the key?\n"
		+ "I guess I'll have to use my old training in lockpicking to get past this.\n"
		+ "\n"
		+ "<i>Looks like they don't always leave the key behind... sigh</i>");
		CanvasScript.SetNotificationBoxCloseText("Damn movies");
		CanvasScript.ShowNotificationBox();

		InputManager.GetInstance()._onNotificationClose.AddListener(ShowAbilityNotification);
	}

	void ShowAbilityNotification()
	{
		GameManager.GetInstance().playerEntity.AddAbility(_newAbility);
		Ability.NewAbilityNotification(_newAbility);
		InputManager.GetInstance()._onNotificationClose.AddListener(GameManager.PrepareLevel);
	}
	
	void Update ()
	{
		
	}
}
