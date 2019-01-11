using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_36_Ice_Script : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger && col.tag == "PLAYER")
        GameManager.GetInstance().playerEntity.Hit(100, null);
    }
}
