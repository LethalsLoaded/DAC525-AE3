using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	void OnEnable()
	{
	}

	[SerializeField]
	public void TestMethod(Touch touch, Ability ability)
	{
		Debug.Log("Yeet");
	}

	[SerializeField]
	public void TestM(Ability ability)
	{

	}
}
