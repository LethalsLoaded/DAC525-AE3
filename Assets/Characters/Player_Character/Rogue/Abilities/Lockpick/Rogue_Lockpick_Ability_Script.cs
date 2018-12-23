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

[CreateAssetMenu(fileName = "Rogue_Lockpick_Ability", menuName = "Character_Abilities/Rogue/Lockpick", order = 3)]
public class Rogue_Lockpick_Ability_Script : Ability
{
	public float raycastLength = 1.0f;

    public override void Start()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public override void Use()
    {
        Destroy(GameObject.FindGameObjectWithTag("KEY_DOOR"));
    }
}
