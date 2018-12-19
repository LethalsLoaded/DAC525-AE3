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

[CreateAssetMenu(fileName = "Rogue_Envenom_Ability", menuName = "Character_Abilities/Rogue/Envenom", order = 3)]
public class Rogue_Envenom_Ability_Script : Ability {

	public float _secondsUntilDeactivate;

    public override void Start()
    {
        // Do stuff here when an ability is SPAWNED into the world
    }

    public override void Update()
    {
		if(!this.isActive) return;
    }

    public override void Use()
    {
		this.isActive = true;
		// start a 15 sec timer
    }
}
