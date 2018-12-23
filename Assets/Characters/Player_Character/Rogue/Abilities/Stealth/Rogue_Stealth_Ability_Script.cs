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

[CreateAssetMenu(fileName = "Rogue_Stealth_Ability", menuName = "Character_Abilities/Rogue/Stealth", order = 2)]
public class Rogue_Stealth_Ability_Script : Ability
{
    public float maxDistanceToEnemy = 1;

    public float movementPenaltyPercentage = 40;

    public override void Start()
    {
        isActive = false;
    }

    public override void Update()
    {
        if(!isActive) return;

        // Check Right
        var hit = Physics2D.Raycast(abilityOwner.transform.position, abilityOwner.transform.right, maxDistanceToEnemy);
        if(hit && hit.collider.tag == "ENTITY")
            EnemyDetectedTooClose(hit.collider);
        else
        {
            // Check Left
            hit = Physics2D.Raycast(abilityOwner.transform.position, -abilityOwner.transform.right, maxDistanceToEnemy);
            if(hit && hit.collider.tag == "ENTITY")
                EnemyDetectedTooClose(hit.collider);
        }
    }

    public void EnemyDetectedTooClose(Collider2D collider)
    {
        Debug.Log("No longer stealthy boy");
        isActive = false;
        var colora = abilityOwner.GetComponent<SpriteRenderer>().color;
        colora.a = 1f;
        abilityOwner.GetComponent<SpriteRenderer>().color = colora;
        abilityOwner.layer = 0;
    }

    public override void Use()
    {
        Debug.Log(abilityOwner.transform.position);
        if (abilityOwner.GetComponent<Rogue_Script>().inCombat) return;

        isActive = !isActive;
        var colora = abilityOwner.GetComponent<SpriteRenderer>().color;
        colora.a = isActive ? 0.5f : 1f;
        abilityOwner.GetComponent<SpriteRenderer>().color = colora;
        abilityOwner.layer = isActive ? 2 : 0;

        Debug.Log(isActive ? "Stealthed" : "Unstealthed");

        // Do stuff when an ability is used (set trajectory etc)
    }
}