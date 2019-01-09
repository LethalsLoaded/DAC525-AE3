using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_33_Spikes_Script : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.transform.tag == "PLAYER")
		{
			Debug.Log("Spikes");
		// Kill player, play death animation
		} 
	}
}
