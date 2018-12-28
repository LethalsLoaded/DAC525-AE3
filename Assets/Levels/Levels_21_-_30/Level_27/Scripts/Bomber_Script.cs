using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber_Script : MonoBehaviour {

	public Transform[] shootSpawn;
	public GameObject diagShot;
	public int timeInterval;
	Transform pos;
	int index;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(DiagShot());
	}
	IEnumerator DiagShot ()
	{
		yield return new WaitForSeconds(timeInterval);
		for (int i = 0; i < shootSpawn.Length; i++)
		{
			index = i;
			pos = shootSpawn[index];
			Instantiate(diagShot, pos.position, pos.rotation);
			Debug.Log(pos.ToString());
		} 
		StartCoroutine(DiagShot());
	}
}
