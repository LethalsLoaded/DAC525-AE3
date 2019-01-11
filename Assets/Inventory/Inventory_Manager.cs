/*
 * This file has been created by Daniel Wesolowski for DAC525-AE3
 * on Thu Nov 22 2018. Everything in this file is written
 * by Daniel Wesolowski unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Manager : MonoBehaviour
{
    private static Inventory_Manager instance;
    public GameObject inventoryWindow;
    public IDictionary<GameObject, Item> inventoryItems = new Dictionary<GameObject, Item>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
    }

    public void AddItemToInventory(Item itemToAdd)
    {
        // Find first free slot
        Debug.Log(inventoryItems.Count);

        foreach (var item in inventoryItems)
        {
            if (item.Value != null) continue;
            // Add an item to it
            inventoryItems[item.Key] = itemToAdd;
            UpdateUI();
            break;
        }
    }

    public void RemoveItemFromInventory(Item itemToRemove)
    {
        foreach (var item in inventoryItems)
        {
            if (item.Value == null || item.Value.GetID() != itemToRemove.GetID()) continue;
            inventoryItems[item.Key] = null;
            UpdateUI();
            break;
        }
    }

    public void RemoveItemFromSlot(GameObject slot)
    {
        if (inventoryItems[slot] == null) return;
        inventoryItems[slot] = null;
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (var item in inventoryItems)
        {
            var slot = item.Key;
            var pairItem = item.Value;

            if (pairItem == null) slot.transform.GetChild(0).GetComponent<Image>().enabled = false;
            else slot.transform.GetChild(0).GetComponent<Image>().enabled = true;

            slot.transform.GetChild(0).GetComponent<Image>().sprite = item.Value != null ? item.Value.itemSprite : null;
        }
    }

    public static Inventory_Manager GetInstance()
    {
        return instance;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}