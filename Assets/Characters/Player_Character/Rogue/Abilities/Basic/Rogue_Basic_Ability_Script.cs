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

[CreateAssetMenu(fileName = "Rogue_Basic_Ability", menuName = "Character_Abilities/Rogue/Basic", order = 1)]
public class Rogue_Basic_Ability_Script : Ability {

	public int _minimumDamage = 1;
	public float _distanceToHit = 1.0f;

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
		// TODO: Check for distance
		// TODO: play animation and sound
		var venom = abilityOwner.GetComponent<Rogue_Script>().GetAbility("Envenom");
		int damage = (int)Random.Range(_minimumDamage, abilityValue + 1);
		if(venom.isActive)
			damage += (int)venom.abilityValue;

		Debug.Log($"Basic attack. Damage dealt: {damage} {(venom.isActive ? "ENVENOMED" : "NO VENOM")}");
    }
}
