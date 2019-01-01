using System.Collections.Generic;
using UnityEngine;

public class Level_3_Chest_Behaviour : MonoBehaviour
{

    public bool canInteract = true;
    public List<Item> itemsToSpawn = new List<Item>();
    private List<GameObject> _spawnedItems = new List<GameObject>();
    public Vector2 minimumThrust, maximumThrust;

	void Start ()
	{
        GameManager.GetInstance().playerCharacter.GetComponent<Rogue_Script>().onInteract.AddListener(OnInteract);
	}
	
	void OnInteract()
    {
        foreach(var item in itemsToSpawn)
        {
            var newItem = Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
            newItem.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(minimumThrust.x,  maximumThrust.x),
                                                                Random.Range(minimumThrust.y, maximumThrust.y));
            _spawnedItems.Add(newItem);
        }
        Invoke("MakeItemsPickupable", 5.0f);
    }

    void MakeItemsPickupable()
    {
        foreach(var item in _spawnedItems)
        {
            item.GetComponent<Gem_Prefab_Script>().canPickup = true;
        }
    }
}
