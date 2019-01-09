using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_35_Possessed_Statue_Script : MonoBehaviour {

	bool isActivated;
	 Color color;
	 public SpriteRenderer[] eyesColor;
	 bool continueCorotuine = true;

	// Use this for initialization
	void Start () 
	{
	//	myRb = GetComponent<Rigidbody2D>();	
		foreach (SpriteRenderer eye in eyesColor)
		{
			color = eye.color;
		}	
		color.a = 0;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(!collider.isTrigger && collider.tag == "PLAYER")
		{
			continueCorotuine = true;
			StartCoroutine (Timer());
		}
	}
	void OnTriggerExit2D (Collider2D collider)
	{
		if(!collider.isTrigger && collider.tag == "PLAYER")
		{
			continueCorotuine = false;
		}
	}
	IEnumerator Timer ()
	{
		yield return new WaitForSeconds(5);
		if(continueCorotuine)
			isActivated = true;
		yield return new WaitForSeconds(1.5f);
		// kill player and change him into a statue
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( isActivated)
		{
			color.a += 0.05f;
			foreach (SpriteRenderer eye in eyesColor)
			{
				eye.color = color;
			}
		}

		// if statue is destroyed
		// doors are open
		// doors.GetComponent<DoorScript>()._isDoorOpen = true;
	}
}
