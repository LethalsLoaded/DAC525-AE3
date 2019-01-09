using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber_Projectile_Scripr : MonoBehaviour {

	Rigidbody2D myRb;
	public float speed;
	public int damage;

	// Use this for initialization
	void Start () 
	{
		myRb = gameObject.GetComponent<Rigidbody2D>();
		myRb.velocity = transform.up * speed;
	}
	
	void OnTriggerEnter2D(Collider2D col)
    {
        if( !col.isTrigger && col.tag == "PLAYER")
        GameManager.GetInstance().playerEntity.Hit(damage, null);
    }
}
