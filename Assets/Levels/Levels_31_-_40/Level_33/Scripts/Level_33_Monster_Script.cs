using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_33_Monster_Script : MonoBehaviour {

	public Transform target;
	//Transform target;
	// Use this for initialization
	void Start ()
	{
		target = GameObject.FindGameObjectWithTag("PLAYER").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//transform.LookAt(target);
		transform.right = target.position - transform.position;
	}
}
