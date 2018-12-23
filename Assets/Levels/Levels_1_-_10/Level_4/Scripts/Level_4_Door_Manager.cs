using UnityEngine;

public class Level_4_Door_Manager : MonoBehaviour {

	public bool playerHasKey;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(playerHasKey)
			Destroy(gameObject);
	}
}
