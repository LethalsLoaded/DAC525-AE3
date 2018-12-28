using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber_Projectile_Scripr : MonoBehaviour {

	Rigidbody2D myRb;
	public float speed;

	// Use this for initialization
	void Start () 
	{
		myRb = gameObject.GetComponent<Rigidbody2D>();
		myRb.velocity = transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
