using UnityEngine;

[CreateAssetMenu(fileName = "Arc_Spider_Arc_Poison_Dart_Ability", menuName = "Character_Abilities/Arc_Spider/Arc_Poison_Dart", order = 0)]
public class Arc_Poison_Dart_Ability_Script : Ability
{

    public float xStrength, yStrength;

    public float amountToSpawn = 3;

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
        throw new System.NotImplementedException();
    }

    public void Use(Transform trans)
    {
        Debug.Log(abilityOwner.gameObject.name);
        for(var i = 0; i < amountToSpawn; i++)
        {
            var dart = Instantiate(abilityPrefab, trans.position, Quaternion.identity);
            if(i == 0)
                dart.GetComponent<Rigidbody2D>().velocity = new Vector2(-xStrength, yStrength);
            else if (i == 1)
                dart.GetComponent<Rigidbody2D>().velocity = new Vector2(-xStrength, 0);
            else if (i == 2)
                dart.GetComponent<Rigidbody2D>().velocity = new Vector2(-xStrength, -yStrength);
        }
    }
}
