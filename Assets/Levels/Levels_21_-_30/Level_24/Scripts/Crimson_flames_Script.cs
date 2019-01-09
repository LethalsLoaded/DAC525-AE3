using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crimson_flames_Script : MonoBehaviour {

	public float jumpForce;
	Rigidbody2D rb;
	int random;
	bool shoot;

	// Use this for initialization
	void Start () 
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		shoot = true;
	}
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger && col.tag == "PLAYER")
        GameManager.GetInstance().playerEntity.Hit(1, null);
    }
	void Update () 
	{
		if (rb.velocity.y == 0 && shoot)
		StartCoroutine(Count());		
	}
	IEnumerator Count()
	{
		shoot = false;
		random = Random.Range(4,6);
		yield return new WaitForSeconds(random);
		rb.AddForce(transform.up * jumpForce);
		shoot = true;
	}
}
