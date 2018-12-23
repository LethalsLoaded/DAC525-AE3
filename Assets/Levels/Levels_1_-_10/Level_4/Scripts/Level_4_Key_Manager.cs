using UnityEngine;

public class Level_4_Key_Manager : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
	{
		Camera.main.GetComponent<Level_4_Level_Manager>().levelDoor.GetComponent<Level_4_Door_Manager>().playerHasKey = true;
		Destroy(gameObject);
	}
}
