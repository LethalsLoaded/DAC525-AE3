using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "PLAYER")
        collider.transform.parent = this.transform;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "PLAYER")
        collider.transform.parent = null;
    }
}
