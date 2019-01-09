using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava_script : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger && col.tag == "PLAYER")
        GameManager.GetInstance().playerEntity.Hit(100, null);
    }
}
