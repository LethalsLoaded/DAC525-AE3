using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magma_Ball_Spawn_Script : MonoBehaviour {

	int random, index;
	public GameObject magmaBall;
	public Transform[] spawnPoint;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(Spawn());
	}
	
	IEnumerator Spawn ()
	{
		random = Random.Range(0,spawnPoint.Length);
		index = random;
		yield return new WaitForSeconds(8);
		Instantiate(magmaBall, spawnPoint[index].position, Quaternion.identity);
		Debug.Log("spawn now");
		StartCoroutine(Spawn());
	}
}
