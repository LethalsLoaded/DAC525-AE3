using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_33_Collapsible_Tile_Script : MonoBehaviour {

	Rigidbody2D myRb;
	public float speed, amount;
	bool isShacking;
	Vector2 startingPos;

	// Use this for initialization
	void Start ()
	{
		myRb = GetComponent<Rigidbody2D>();
		startingPos.x = transform.position.x;
	}

	void Update ()
	{
		if (isShacking)
		transform.position = new Vector2( startingPos.x + Mathf.Sin(Time.time * speed) * amount, transform.position.y);
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.transform.tag == "PLAYER")
		{
			StartCoroutine( Collapse());
			isShacking = true;
		}
	}

	IEnumerator Collapse ()
	{
		yield return new WaitForSeconds(2);
		myRb.bodyType = RigidbodyType2D.Dynamic;
		isShacking = false;
		gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		yield return new WaitForSeconds(5);
		Destroy(this.gameObject);
	}
	
}
