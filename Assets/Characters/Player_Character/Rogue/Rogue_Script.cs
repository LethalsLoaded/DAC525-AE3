﻿/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using UnityEngine;

public class Rogue_Script : Entity
{
    private bool _moveLeft;
    private bool _moveRight;
    public float _rayLength = 1;

    public bool inCombat = false;

    private Rogue_Script _instance;

    protected override void OnSpawn()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        // Do stuff when entity is spawned
        foreach (var ability in entityAbilities)
            ability.abilityOwner = gameObject;

        if(GameLevel.GetActiveLevel() != null)
            GameObject.FindGameObjectWithTag("PLAYER").transform.position = GameLevel.GetActiveLevel().levelStartPoint;

        gameObject.SetActive(false);
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

    public void MoveLeft()
    {
        _moveLeft = !_moveLeft;
        if (_moveLeft) entityFacingDirection = Direction.WEST;
    }

    public void MoveRight()
    {
        _moveRight = !_moveRight;
        if (_moveRight) entityFacingDirection = Direction.EAST;
    }

    public void Jump()
    {
        //if(isInTheAir && blink is not active) UseAbility(GetAbility"Blink");
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, entityJumpStrength));
    }

    public void Attack()
    {
        if (GetAbility("Stealth").isActive) // AND ENEMY IS X AWAY AND FACING RIGHT WAY
        {
            UseAbility(GetAbility("Backstab"));
            return;
        }

        UseAbility(GetAbility("Basic"));
        //if(isInTheAir) UseAbility(GetAbility("Dagger Throw"));
    }

    private void Flip()
    {
        var playerRigidBody = GetComponent<Rigidbody2D>();
        if (playerRigidBody.velocity.x > 0 && transform.localScale.x > 0 ||
            playerRigidBody.velocity.x < 0 && transform.localScale.x < 0)
        {
            Vector2 vector = transform.localScale;
            vector.x *= -1;
            transform.localScale = vector;
        }
    }

    // Necessary to execute 'OnSpawn()'
    private void Start()
    {
        OnSpawn();
    }

    private void Update()
    {
        // entityIsOnGround = Physics2D.Linecast(transform.position, transform.position + new Vector3(0, _rayLength, 0),
        //     1 << LayerMask.NameToLayer("Level_Layer"));

        entityIsOnGround = Physics2D.Raycast(transform.position, -transform.up, _rayLength, 1 << LayerMask.NameToLayer("Land"));
        Debug.DrawRay(transform.position, -transform.up, Color.gray);

        if (_moveRight && !_moveLeft)
            GetComponent<Rigidbody2D>().velocity = new Vector2(1 * entitySpeed, GetComponent<Rigidbody2D>().velocity.y);

        else if (!_moveRight && _moveLeft)
            GetComponent<Rigidbody2D>().velocity =
                new Vector2(-1 * entitySpeed, GetComponent<Rigidbody2D>().velocity.y);

        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
    }
}