using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Script : Entity {

	Rigidbody2D myRb;
    public float rayLength;
	 
	protected override void Start ()
	{
		myRb = GetComponent<Rigidbody2D>();
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.name == "Block")
			Flip();
        
        if (col.isTrigger && col.tag == "PLAYER")
            {
                GetAbility("Corrupted Touch").Use();
            }
        if (col.name == "Destroy")
            Destroy(this.gameObject);
	}
	private void Flip()
    {
		entitySpeed = -entitySpeed;
        Vector2 VEC = transform.localScale;
        VEC.x *= -1;
        transform.localScale = VEC;
    }
    protected void FixedUpdate ()
    {
        if(Physics2D.Linecast(transform.position,
       			 transform.position + new Vector3(0,-rayLength,0),
       			 1 << LayerMask.NameToLayer("Land")))
            myRb.velocity = new Vector2(entitySpeed , myRb.velocity.y);
        
    }
    protected override void OnDeath(Entity entityKiller = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnHit(Entity entityDamager)
    {
        StartCoroutine(Blink());
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

}
