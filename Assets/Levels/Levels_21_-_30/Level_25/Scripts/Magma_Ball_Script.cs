using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magma_Ball_Script : MonoBehaviour {

	Rigidbody2D ballRb;
	public float forwardForce, jumpForce;

	// Use this for initialization
	void Start () 
	{
		ballRb = gameObject.GetComponent<Rigidbody2D>();
		if (gameObject.transform.position.x > 0)
			forwardForce = -forwardForce;
		ballRb.AddForce(new Vector2 (forwardForce,0));
	}
	
	// Update is called once per frame
		void OnCollisionEnter2D (Collision2D col)
	{
		ballRb.AddForce(new Vector2 (0,0));
		if (col.transform.name == "Outer")
			ballRb.AddForce(new Vector2 (forwardForce,jumpForce));
		if (col.transform.name == "Wall")
			Destroy(this.gameObject);		
	}
	
}
