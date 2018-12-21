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
public class Rogue_Basic_Ability_Script : Ability
{
    public float _distanceToHit = 1.0f;

    public int _minimumDamage = 1;

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
        isActive = true;
        var direction = Vector3.zero;
        if (abilityOwner.GetComponent<Rogue_Script>().entityFacingDirection == Direction.EAST)
            direction = abilityOwner.transform.right;
        else
            direction = -abilityOwner.transform.right;

        Debug.Log(direction);
        Debug.DrawRay(abilityOwner.transform.position, direction, Color.red, 2.0f, false);
        var hit = Physics2D.Raycast(abilityOwner.transform.position, direction, 10.0f);

        if (!hit
            || hit.collider.tag != "ENTITY"
            || hit.collider.gameObject.GetComponent<Entity>().isDead)
            return;
        // TODO: play animation and sound

        var venom = abilityOwner.GetComponent<Rogue_Script>().GetAbility("Envenom");
        var damage = (int) Random.Range(_minimumDamage, abilityValue + 1);
        if (venom.isActive)
            damage += (int) venom.abilityValue;

        //test
        hit.collider.GetComponent<Entity>().Hit(damage, abilityOwner.GetComponent<Entity>());
        Debug.Log(
            $"Basic attack. Damaged {hit.collider.name} for {damage} {(venom.isActive ? "ENVENOMED" : "NO VENOM")}");
    }
}