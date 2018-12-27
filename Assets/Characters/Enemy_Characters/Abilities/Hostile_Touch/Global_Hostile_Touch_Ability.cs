using UnityEngine;

[CreateAssetMenu(fileName = "Global_Hostile_Touch_Ability", menuName = "Character_Abilities/Global/Hostile_Touch", order = 0)]
public class Global_Hostile_Touch_Ability : Ability
{
    bool firstTime;
    public override void Start()
    {
        
    }

    public override void Update()
    {
    }

    public override void Use()
    {
        GameManager.GetInstance().playerEntity.Hit((int)abilityValue, abilityOwner.GetComponent<Entity>());
    }
}
