using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser_script : Entity {

	public float rayLenght;
	public bool isChasing;
	enum State{ IDLE, CHASING, RETURNING }
	Vector2 originPoint;
	Rigidbody2D myRb;
	State myState;
	// Use this for initialization
	protected override void Start () 
	{
		myState = State.IDLE;
		myRb = GetComponent<Rigidbody2D>();
		originPoint = transform.position;
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.transform.name == "Block" )
		{
			Debug.Log("return");
			myState = State.IDLE;
			myRb.velocity = Vector3.zero;
			StartCoroutine (Return());
		}

		if (col.transform.name == "Stop" )
		{
			Debug.Log("Idle");
			myState = State.IDLE;
			myRb.velocity = Vector3.zero;
			StartCoroutine (Stop());
		}
		if( col.tag == "PLAYER")
		{
			Debug.Log("use chaser ability");
       		GetAbility("Chaser Touch").Use();
		}
	}
	IEnumerator Return()
	{
		yield return new WaitForSeconds(1);
		rayLenght = -rayLenght;
			entitySpeed = -entitySpeed;
			myState = State.RETURNING;
		//	myRb.AddForce(new Vector2(2,0));
	}
	IEnumerator Stop()
	{
		yield return new WaitForSeconds(0.2f);
		rayLenght = -rayLenght;
			entitySpeed = -entitySpeed;
			myState = State.IDLE;
	}
	protected override void Update () 
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right ,distance:rayLenght);
		Debug.DrawLine(transform.position, transform.position + new Vector3(rayLenght,0,0), Color.yellow);
		if (hit.collider.tag == "PLAYER")
		{
			myState = State.CHASING;
			Debug.Log(hit.collider.name);
		}

		if (myState == State.IDLE)
			myRb.velocity = new Vector2 (0,0);
		else if (myState == State.CHASING)
			myRb.velocity = new Vector2(entitySpeed,0);
		else if (myState == State.RETURNING)
			myRb.velocity = new Vector2(entitySpeed,0);
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
        StartCoroutine(Blink());
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        throw new System.NotImplementedException();
    }
}
