using UnityEngine;

public class Level_2_Level_Manager : MonoBehaviour {

	public Ability _newAbility;

	void Start ()
	{
		CanvasScript.SetNotificationBoxTitle("New stuff!");
		CanvasScript.SetNotificationBoxText("To get past this level you need to use your newely acquired <b>STEALTH</b> ability.\n"
		+ "There is a dummy who is patrolling the way to the door who will not hesitate to remove your life.\n"
		+ "\n"
		+ "Use the ability wisely!");
		CanvasScript.SetNotificationBoxCloseText("Like a ninja!");
		CanvasScript.ShowNotificationBox();

		InputManager.GetInstance()._onNotificationClose.AddListener(ShowAbilityNotification);
	}

	void ShowAbilityNotification()
	{
		GameManager.GetInstance().playerEntity.AddAbility(_newAbility);
		Ability.NewAbilityNotification(_newAbility);
		InputManager.GetInstance()._onNotificationClose.AddListener(GameManager.PrepareLevel);

		var enemyEntity = GameObject.Find("LEVEL_ENEMIES").transform.GetChild(0).GetComponent<Entity>();
		enemyEntity.UseAbility(enemyEntity.entityAbilities[0]);
		Debug.Log(enemyEntity.entityAbilities[0].isActive);
	}

	
	void Update ()
	{
		
	}
}
