using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mentally_Insane_Cloud_Elemental_Ability", menuName = "Character_Abilities/Mentally_Insane_Cloud_Elemental/Drop_Aero_Bomb",
    order = 1)]
public class Mentally_Insane_Cloud_Elemental_Ability_Script : Ability {

    public float amountToSpawn = 2;
    public Vector2 minimumThrust, maximumThrust;
    private IList<GameObject> _spawnedBalls = new List<GameObject>();

    public override void Start()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        foreach(var item in _spawnedBalls)
        {
            if(item.transform.position.y < -50)
            {
                _spawnedBalls.Remove(item);
                Destroy(item);
                break;
            }
        }
    }

    public override void Use()
    {
        if(abilityOwner.GetComponent<Entity>().isDead) return;
        for(var i = 0; i < amountToSpawn; i++)
        {
            var newObject = Instantiate(abilityPrefab, abilityOwner.transform.position, Quaternion.identity);
            var thrustVector = new Vector2(Random.Range(minimumThrust.x, maximumThrust.x),
                                            Random.Range(minimumThrust.y, maximumThrust.y));
            Debug.Log(thrustVector);
            newObject.GetComponent<Rigidbody2D>().velocity = (thrustVector);
            _spawnedBalls.Add(newObject);
        }
    }
}
