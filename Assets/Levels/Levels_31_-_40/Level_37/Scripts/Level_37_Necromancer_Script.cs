using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_37_Necromancer_Script : Entity {

	public GameObject zombie;
	public Transform spawnPoint;
    protected override void OnDeath(Entity entityKiller = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnHit(Entity entityDamager)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

    protected override void Start ()
	{
		StartCoroutine("AnimateDead");
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		if( col.tag == "PLAYER")
		{
			
		}
	}

	IEnumerator AnimateDead ()
	{
		Debug.Log("Animate Dead");
		yield return new WaitForSeconds(8);
		Instantiate( zombie, spawnPoint.transform.position, spawnPoint.transform.rotation);
	}
	
}
