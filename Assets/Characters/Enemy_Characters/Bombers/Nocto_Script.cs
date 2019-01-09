using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nocto_Script : Entity {

	public Transform target, spawnPoint;
	public GameObject projectile;
	// Use this for initialization
	protected override void Start () 
	{
		StartCoroutine(Shoot());
		target = GameObject.FindGameObjectWithTag("PLAYER").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//transform.LookAt(target);
		transform.right = target.position - transform.position;
	}
	void OnTriggerEnter2D(Collider2D col)
    {
        if( !col.isTrigger && col.tag == "PLAYER")
       		GetAbility("Nocto Touch").Use();
    }
	IEnumerator Shoot()
	{
		yield return new WaitForSeconds(7);
		Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
		StartCoroutine(Shoot());
	}

    protected override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

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
}
