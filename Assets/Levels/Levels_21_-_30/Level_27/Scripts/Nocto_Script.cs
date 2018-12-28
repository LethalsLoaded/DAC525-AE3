using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nocto_Script : MonoBehaviour {

	public Transform target, spawnPoint;
	public GameObject projectile;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(Shoot());
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//transform.LookAt(target);
		transform.right = target.position - transform.position;
	}
	IEnumerator Shoot()
	{
		yield return new WaitForSeconds(7);
		Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
		StartCoroutine(Shoot());
	}
}
