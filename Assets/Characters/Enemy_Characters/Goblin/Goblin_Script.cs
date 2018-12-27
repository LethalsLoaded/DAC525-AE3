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

public class Goblin_Script : Entity
{

	public Vector2 pointOne, pointTwo;
    [HideInInspector]

    protected override void OnDeath(Entity entityKiller = null)
    {
    }

    protected override void OnHit(Entity entityDamager)
    {
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
    }

    protected override void OnSpawn()
    {
    }

    bool _movingToPointsTwo = true;

	protected override void Update()
	{
        base.Update();
        if(isDead) return;
        var speed = entitySpeed * Time.deltaTime;
        if(_movingToPointsTwo)
            transform.position = Vector2.MoveTowards(transform.position, pointTwo, speed);
        else
            transform.position = Vector2.MoveTowards(transform.position, pointOne, speed);

        if(((Vector2)transform.position == pointTwo))
            _movingToPointsTwo = false;
        else if (((Vector2)transform.position == pointOne))
            _movingToPointsTwo = true;
	}

    protected override void Start()
    {
        base.Start();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(isDead) return;
        if(collider == null || collider.tag != "PLAYER") return;
        
        if(HasAbility("Hostile Touch"))
            UseAbility(GetAbility("Hostile Touch"));

    }
}