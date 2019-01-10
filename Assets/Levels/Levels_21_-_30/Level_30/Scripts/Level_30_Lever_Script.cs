using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_30_Lever_Script : MonoBehaviour {

	bool isOpening, isClosing;
	public float speed, maxY, minY;
	Rigidbody2D doorsRb;
	public GameObject doors;
	// Use this for initialization
	void Start () 
	{
		doorsRb = doors.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isOpening)
		doorsRb.velocity = transform.up * speed;
		else if (isClosing)
		doorsRb.velocity = -transform.up * speed;
		
		if (doors.transform.position.y >= maxY && isOpening)
		doorsRb.velocity = Vector3.zero;
		else if (doors.transform.position.y <= minY && isClosing)
		{
			doorsRb.velocity = Vector3.zero;
			doors.transform.position = new Vector2(doors.transform.position.x, minY);
		}
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "PLAYER" && !isOpening && !isClosing)
		StartCoroutine(OpenDoor());
	}
	IEnumerator OpenDoor ()
	{
		isClosing = false;
		isOpening = true;
		yield return new WaitForSeconds(8);
		Debug.Log("close");
		isOpening = false;
		isClosing = true;
		yield return new WaitForSeconds(2);
		isClosing = false;		
	}
}
