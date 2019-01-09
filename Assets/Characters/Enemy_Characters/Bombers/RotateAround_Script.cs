using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround_Script : MonoBehaviour {

	public float speed;
	public Transform target;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.RotateAround(target.position, new Vector3(0,0,1), speed);
		transform.rotation = Quaternion.identity;
	}
}
