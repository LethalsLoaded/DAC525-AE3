using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_33_Monster_Script : MonoBehaviour {

	GameObject player;
	//Transform target;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("PLAYER");
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//transform.LookAt(target);
		transform.right = player.transform.position - transform.position;
	}
}
