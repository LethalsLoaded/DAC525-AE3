using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_trap_Script : MonoBehaviour {

	bool isActivated;
	bool isShooting;
	public Transform spawnPoint;
	public GameObject arrow;
	public float arrowSpeed;

	// Use this for initialization
	void Start () 
	{
		isActivated = false;
		isShooting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActivated)
		StartCoroutine(Shoot());
		
	}
 IEnumerator Shoot()
 {
	 isActivated = false;
	 yield return new WaitForSeconds(2);
	 Debug.Log("qwerty");
	 Instantiate(arrow,spawnPoint);
	isActivated = true;
 }
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "PLAYER")
		isActivated = true;
	}
}
