using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer_Script : Entity {

	public GameObject zombie, door;
	public Transform spawnPoint;
    public float rayLenght;
    [SerializeField]
    bool isDeathRay;
    bool shootRay;

    protected override void OnDeath(Entity entityKiller = null)
    {
        door.GetComponent<DoorScript>()._isDoorOpen = true;
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
        isDeathRay = false;
	}
    protected void OnTriggerEnter2D (Collider2D hit)
    {
        
		if (hit.isTrigger && hit.tag == "PLAYER")
		{
            StopCoroutine("AnimateDead");
            if( !isDeathRay)
            {
                StartCoroutine("DeathRay");
                Debug.Log("start death ray coroutine");
            }
            // else
            // {
            //     // kill everything
            //     // play ray animation
            //     Debug.Log("playe death ray animation");
            // }
		}
        if (shootRay)
        {
            GameManager.GetInstance().playerEntity.Hit(10,null);
            hit.gameObject.GetComponent<Zombie_Script>().isDead = true;
        }
    }
    void OnTriggerExit2D (Collider2D col)
        {
            if (col.isTrigger && col.tag == "PLAYER")
            {
                isDeathRay = false;
                StopCoroutine("DeathRay");
                StartCoroutine("AnimateDead");
            }
        }
        
	IEnumerator AnimateDead()
	{
		Debug.Log("Animate Dead");
		yield return new WaitForSeconds(8);
		Instantiate( zombie, spawnPoint.transform.position, spawnPoint.transform.rotation);
        StartCoroutine("AnimateDead");
	}
    IEnumerator DeathRay()
    {
        isDeathRay = true;
        yield return new WaitForSeconds(15);
        Debug.Log("playe death ray animation");
        // kill everything
        // play ray animation
        // use ability
        yield return new WaitForSeconds(3);
        isDeathRay = false;
    }
	
}
