using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_33_Light_Script : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("PLAYER");
		this.gameObject.transform.parent = player.transform;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "DOOR")
			this.gameObject.transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
