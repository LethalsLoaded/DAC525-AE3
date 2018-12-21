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

[CreateAssetMenu(fileName = "Template_Enemy_Attack_Ability", menuName = "Character_Abilities/Template_Enemy/Attack",
    order = 1)]
public class TemplateEnemyAttackAbilityScript : Ability
{
    public override void Start()
    {
        // Do stuff here when an ability is SPAWNED into the world
    }

    public override void Update()
    {
        // Do stuff on update which is every frame
    }

    public override void Use()
    {
        // Do stuff when an ability is used (set trajectory etc)
    }
}