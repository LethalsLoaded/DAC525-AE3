using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_34_Mushroom_Script : MonoBehaviour {

	public float radius, mvmtSpeed;
	Rigidbody2D myRb;
	 
	void Start ()
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
	void Update ()
	{
		myRb.velocity = Vector2.right * mvmtSpeed;

		Collider2D [] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
			foreach (Collider2D col in colliders)
			{
				if (col != null && col.name != "Mushroom")
				{
					if (col.tag == "PLAYER")
						Debug.Log(col.name);
				}
			}

		// RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,radius,Vector2.right,distance);
		// //(transform.position, Vector2.right ,distance:radius);
		// 	foreach (RaycastHit2D hit in hits)
		// 	{
		// 		if (hit.collider != null)
		// 			Debug.Log(hit.collider.name);
		// 	}
			
		
		Debug.DrawLine(transform.position, transform.position + new Vector3(radius,0,0), Color.yellow);	
	}
}
