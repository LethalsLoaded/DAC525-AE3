﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_35_Random_Statue_Script : MonoBehaviour {

	GameObject[] statues;
	int MyIndex;
	GameObject randStatue;

	// Use this for initialization
	void Start ()
	{
		statues = GameObject.FindGameObjectsWithTag("STATUE");
		MyIndex = Random.Range(0,(statues.Length - 1));
		randStatue = statues[MyIndex];
		randStatue.GetComponent<Level_35_Possessed_Statue_Script>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}