using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_32_Chaser_script : MonoBehaviour {

	public float rayLenght, speed;
	public bool isChasing;
	enum State{ IDLE, CHASING, RETURNING }
	Vector2 originPoint;
	Rigidbody2D myRb;
	State myState;
	// Use this for initialization
	void Start () 
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
	}
	IEnumerator Return()
	{
		yield return new WaitForSeconds(1);
		rayLenght = -rayLenght;
			speed = -speed;
			myState = State.RETURNING;
		//	myRb.AddForce(new Vector2(2,0));
	}
	IEnumerator Stop()
	{
		yield return new WaitForSeconds(0.2f);
		rayLenght = -rayLenght;
			speed = -speed;
			myState = State.IDLE;
	}
	void Update () 
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
			myRb.velocity = new Vector2(speed,0);
		else if (myState == State.RETURNING)
			myRb.velocity = new Vector2(speed,0);
	}
}
