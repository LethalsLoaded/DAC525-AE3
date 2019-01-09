using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy_Golem_Script : Entity {

	//public float jumpForce, forwardForce;
	Rigidbody2D golemRB;
	public GameObject doors;

	// Use this for initialization
	protected override void Start () 
	{
		golemRB = gameObject.GetComponent<Rigidbody2D>();
	}
	
	void OnCollisionEnter2D (Collision2D col)
	{
		golemRB.AddForce(new Vector2 (0,0));
		if (col.transform.name == "Outer")
		golemRB.AddForce(new Vector2 (entitySpeed,entityJumpStrength));
		if (col.transform.name == "Wall")
		Flip();
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		GetAbility("Golem Touch").Use();
	}
	void Flip ()
	{
		entitySpeed = -entitySpeed;
		gameObject.transform.rotation = new Quaternion(0,180,0,0);
	}

    protected override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDeath(Entity entityKiller = null)
    {
       doors.GetComponent<DoorScript>()._isDoorOpen = true;
    }

    protected override void OnHit(Entity entityDamager)
    {
        StartCoroutine(Blink());
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        throw new System.NotImplementedException();
    }
}
