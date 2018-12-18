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
using UnityEngine.EventSystems;

public class Rogue_Script : Entity
{
    bool _moveRight = false;
    bool _moveLeft = false;
    public float _rayLength = 1;

    public bool inCombat = false;

    protected override void OnSpawn()
    {
        // Do stuff when entity is spawned
        foreach(var ability in this.entityAbilities)
        {
            ability.abilityOwner = gameObject;
        }

    }

    protected override void OnDeath(Entity entityKiller = null)
    {
        // Do stuff when killed (NOT DESTROYED!)
    }

    protected override void OnHit(Entity entityDamager)
    {
        // Do stuff when hit by entityDamager
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        // Do stuff when interacted with by entityInteracter
    }

    public void UseAbility(Ability ability)
    {
        ability.Use();
    }

    public void MoveLeft()
    {
        _moveLeft = !_moveLeft;
    }

    public void MoveRight()
    {
        _moveRight = !_moveRight;
    }

    public void Jump()
    {
        //if(isInTheAir && blink is not active) UseAbility(GetAbility"Blink");
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, this.entityJumpStrength));
    }

    public void Attack()
    {
        if(GetAbility("Stealth").isActive) // AND ENEMY IS X AWAY AND FACING RIGHT WAY
            UseAbility(GetAbility("Backstab"));

        //if(isInTheAir) UseAbility(GetAbility("Dagger Throw"));
    }

    private void Flip()
    {
        var playerRigidBody = GetComponent<Rigidbody2D>();
        if(playerRigidBody.velocity.x > 0 && transform.localScale.x > 0 || playerRigidBody.velocity.x < 0 && transform.localScale.x < 0)
        {
        Vector2 vector = transform.localScale;
        vector.x *= -1;
        transform.localScale = vector;
        }
    }

    // Necessary to execute 'OnSpawn()'
    void Start()
        => OnSpawn();

    void Update()
    {
        entityIsOnGround = Physics2D.Linecast(transform.position, transform.position + new Vector3(0, _rayLength, 0), 1 << LayerMask.NameToLayer("Land"));

        if(_moveRight && !_moveLeft)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1 * this.entitySpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (!_moveRight && _moveLeft)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * this.entitySpeed, GetComponent<Rigidbody2D>().velocity.y);
        }        
    }
}
