using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Script : MonoBehaviour {

	Rigidbody2D rb;
	public float speed;

	// Use this for initialization
	void Start () 
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2 (speed,0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (gameObject.transform.position.x > 40)
		Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger && col.tag == "PLAYER")
        GameManager.GetInstance().playerEntity.Hit(1, null);
    }
}
