using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magma_Ball_Script : Entity {

	Rigidbody2D ballRb;
	public float forwardForce, jumpForce;

	// Use this for initialization
	protected override void Start () 
	{
		ballRb = gameObject.GetComponent<Rigidbody2D>();
		if (gameObject.transform.position.x > 0)
			entitySpeed = -entitySpeed;
		ballRb.AddForce(new Vector2 (entitySpeed,0));
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		GetAbility("Magma Ball Touch").Use();
	}
		void OnCollisionEnter2D (Collision2D col)
	{
		ballRb.AddForce(new Vector2 (0,0));
		if (col.transform.name == "Outer")
			ballRb.AddForce(new Vector2 (entitySpeed, entityJumpStrength));
		if (col.transform.name == "Wall")
			Destroy(this.gameObject);		
	}

    protected override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDeath(Entity entityKiller = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnHit(Entity entityDamager)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        throw new System.NotImplementedException();
    }
}
