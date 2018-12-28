/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rogue_Script : Entity
{
    public UnityEvent onJump, onInteract;
    private bool _moveLeft;
    private bool _moveRight;
    public float _rayLength = 1;
    private bool _canInteract;

    private bool _lockpicking = false;

    public bool inCombat = false;

    private Rogue_Script _instance;

    protected override void OnSpawn()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);


        if(GameLevel.GetActiveLevel() != null)
            GameObject.FindGameObjectWithTag("PLAYER").transform.position = GameLevel.GetActiveLevel().levelStartPoint;

        gameObject.SetActive(false);
    }

    protected override void OnDeath(Entity entityKiller = null)
    {
        // Do stuff when killed (NOT DESTROYED!)

        // TODO: Load some scene that you lost a life or some crap
        GameManager.GetInstance().RemoveLife();
        GameLevel.RestartLevel();
    }

    protected override void OnHit(Entity entityDamager)
    {
        // Do stuff when hit by entityDamager
        StartCoroutine(Blink());
        StopCoroutine(LockpickCountdown());
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
        if(_canInteract)
        {
            onInteract.Invoke();
            return;
        }

        //if(isInTheAir && blink is not active) UseAbility(GetAbility"Blink");
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, entityJumpStrength));
        onJump.Invoke();
    }

    public void Attack()
    {
        if (HasAbility("Stealth") && HasAbility("Backstab") && GetAbility("Stealth").isActive) // AND ENEMY IS X AWAY AND FACING RIGHT WAY
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
    protected override void Start()
    {
        base.Start();
        OnSpawn();
    }

    private IEnumerator LockpickCountdown()
    {
        Debug.Log("owo");
        _lockpicking = true;
        var lockpick = GetAbility("Lockpick");
        bool stop = false;
        for(var i = 0; i < lockpick.abilityValue * 2; i++)
        {
            if(gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero) stop = true;
            if(stop) break;

            yield return new WaitForSeconds(0.5f);
        }

        if(stop)
        {
            _lockpicking = false;
        }
        else
        {
            lockpick.Use();
        }
    }

    protected override void Update()
    {
        base.Update();
        // entityIsOnGround = Physics2D.Linecast(transform.position, transform.position + new Vector3(0, _rayLength, 0),
        //     1 << LayerMask.NameToLayer("Level_Layer"));

        // ********* MOVEMENT STUFF ************** //

        entityIsOnGround = Physics2D.Raycast(transform.position, -transform.up, _rayLength, 1 << LayerMask.NameToLayer("Land"));
        Debug.DrawRay(transform.position, -transform.up, Color.gray);

        if (_moveRight && !_moveLeft)
            GetComponent<Rigidbody2D>().velocity = new Vector2(1 * entitySpeed, GetComponent<Rigidbody2D>().velocity.y);

        else if (!_moveRight && _moveLeft)
            GetComponent<Rigidbody2D>().velocity =
                new Vector2(-1 * entitySpeed, GetComponent<Rigidbody2D>().velocity.y);
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

        // ********* LOCKPICK STUFF ************** //

        if(HasAbility("Lockpick") && GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            var hit = Physics2D.Raycast(transform.position, transform.right, 1.0f);
            if(hit && hit.collider.tag == "KEY_DOOR")
                StartCoroutine(LockpickCountdown());
            else
            {
                hit = Physics2D.Raycast(transform.position, -transform.right, 1.0f);
                if(hit && hit.collider.tag == "KEY_DOOR")                
                    StartCoroutine(LockpickCountdown());
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag != "INTERACTABLE") return;
        _canInteract = true;
        // TODO: Change UI icon/button?
        Debug.Log("Can interact");
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag != "INTERACTABLE") return;
        _canInteract = false;
        Debug.Log("Cant interact");
    }
}