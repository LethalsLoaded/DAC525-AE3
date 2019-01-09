using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_35_Statue_Script : Entity {
	public GameObject doors;
    protected override void OnDeath(Entity entityKiller = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnHit(Entity entityDamager)
    {
        if (gameObject.GetComponent<Level_35_Possessed_Statue_Script>().enabled == false)
		{
			// change player into statue
			Debug.Log("Change player into statue");
		}
		else
			doors.GetComponent<DoorScript>()._isDoorOpen = true;
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger || col.tag != "PLAYER") return;
        if(HasAbility("Hostile Touch")) GetAbility("Hostile Touch").Use();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
