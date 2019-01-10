using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_30_Doors_Script : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if( !col.isTrigger && col.tag == "PLAYER")
		{
			Debug.Log("kill me");
       		GameManager.GetInstance().playerEntity.Hit(100, null);
		}
    }
}
