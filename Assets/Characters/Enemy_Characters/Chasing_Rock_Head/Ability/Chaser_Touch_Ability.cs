using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chaser_Ability", menuName = "Character_Abilities/Chaser_Rock_Head/Chaser_Touch", order = 0)]
public class Chaser_Touch_Ability : Ability
 {
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
        GameManager.GetInstance().playerEntity.Hit((int)Random.Range(2,3), abilityOwner.GetComponent<Entity>());
    }

    
}
