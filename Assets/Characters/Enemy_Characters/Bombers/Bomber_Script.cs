using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber_Script : Entity {

	public Transform[] shootSpawn;
	public GameObject diagShot;
	public int timeInterval;
	Transform pos;
	int index;

	// Use this for initialization
	protected override void Start () 
	{
		StartCoroutine(DiagShot());
	}
	void OnTriggerEnter2D(Collider2D col)
    {
        if( !col.isTrigger && col.tag == "PLAYER")
        GetAbility("Bomber Touch").Use();
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
