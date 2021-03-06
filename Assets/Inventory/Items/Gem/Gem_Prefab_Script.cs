﻿using UnityEngine;

public class Gem_Prefab_Script : MonoBehaviour {

	public bool canPickup = false;
    public Item itemToAdd;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!canPickup) return;
        if(collider.isTrigger || collider.tag != "PLAYER") return;

        collider.GetComponent<Inventory_Manager>().AddItemToInventory(itemToAdd);
        Destroy(gameObject);
    }
}
