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
public class Rogue_Stealth_Ability_Script : Ability {

    public float movementPenaltyPercentage = 40;
    public float maxDistanceToEnemy = 1;

    public override void Start()
    {
      isActive = false;
    }

    public override void Update()
    {
      Debug.Log($"{isActive}");
		  //Debug.Log($"{abilityName} ({isActive}) | Pos: " + abilityOwner.transform.position);
    }

    public override void Use()
    {
      Debug.Log(abilityOwner.transform.position);
      if(abilityOwner.GetComponent<Rogue_Script>().inCombat) return;

      isActive = !isActive;
      var colora = abilityOwner.GetComponent<SpriteRenderer>().color;
      colora.a = isActive ? 0.5f : 1f;
      abilityOwner.GetComponent<SpriteRenderer>().color = colora;

      Debug.Log(isActive ? "Stealthed" : "Unstealthed");
        
		// Do stuff when an ability is used (set trajectory etc)
    }
}
