/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using UnityEngine;

public class Dummy_Script : Entity
{
    protected override void OnDeath(Entity entityKiller = null)
    {
    }

    protected override void OnHit(Entity entityDamager)
    {
        Debug.Log($"I have been hit by {entityDamager.name}!");
        StartCoroutine(Blink());
        // TODO: Play animation and crap
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
    }

    protected override void OnSpawn()
    {
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if(gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0.5f)
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody2D>().velocity, 0.5f);
    }
}