using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy_Golem_Script : MonoBehaviour {

	public float jumpForce, forwardForce;
	public float rayLength;
	Rigidbody2D golemRB;

	// Use this for initialization
	void Start () 
	{
		golemRB = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	void OnCollisionEnter2D (Collision2D col)
	{
		golemRB.AddForce(new Vector2 (0,0));
		if (col.transform.name == "Outer")
		golemRB.AddForce(new Vector2 (forwardForce,jumpForce));
		if (col.transform.name == "Wall")
		Flip();
	}
	void Flip ()
	{
		forwardForce = -forwardForce;
		gameObject.transform.rotation = new Quaternion(0,180,0,0);
	}
}
