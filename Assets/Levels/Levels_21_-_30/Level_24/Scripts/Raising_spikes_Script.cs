﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raising_spikes_Script : MonoBehaviour {

	public Sprite hiddenSpikes, spikes;
	private Collider2D myCollider;
	bool repeat;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (RiseSpikes());   
		myCollider = gameObject.GetComponent<BoxCollider2D>();
	}
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger && col.tag == "PLAYER")
        GameManager.GetInstance().playerEntity.Hit(1, null);
    }
	IEnumerator RiseSpikes ()
	{
		yield return new WaitForSeconds(3);
		gameObject.GetComponent<SpriteRenderer>().sprite = spikes;
		myCollider.enabled = !myCollider.enabled;
		Debug.Log("rise spikes");
		StartCoroutine(HideSpikes());
	}

	IEnumerator HideSpikes ()
	{
		yield return new WaitForSeconds(1.5f);
		gameObject.GetComponent<SpriteRenderer>().sprite = hiddenSpikes;
		myCollider.enabled = !myCollider.enabled;
		Debug.Log("hide spikes");
		StartCoroutine(RiseSpikes());
	}
}
