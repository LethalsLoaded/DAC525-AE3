using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Script : Entity {

	public float radius, mvmtSpeed;
	Rigidbody2D myRb;
	 
	protected override void Start ()
	{
		myRb = GetComponent<Rigidbody2D>();
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.name == "Block")
			Flip();
	}
	private void Flip()
    {
		mvmtSpeed = -mvmtSpeed;
        Vector2 VEC = transform.localScale;
        VEC.x *= -1;
        transform.localScale = VEC;
    }

	// Update is called once per frame
	protected override void Update ()
	{
		myRb.velocity = Vector2.right * mvmtSpeed;

		Collider2D [] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
			foreach (Collider2D col in colliders)
			{
				if (col.tag == "PLAYER" || col.name == "Mushroom")
				{
					Debug.Log(col.name);
					GameManager.GetInstance().playerEntity.Hit(100, null);
					col.gameObject.GetComponent<Mushroom_Script>().isDead = true;
					// destroy player and mushroms if they are in radius
				}
			}			
		
		Debug.DrawLine(transform.position, transform.position + new Vector3(radius,0,0), Color.yellow);	
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
