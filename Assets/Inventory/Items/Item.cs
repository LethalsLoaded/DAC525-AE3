﻿/*
 * This file has been created by Daniel Wesolowski for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Daniel Wesolowski unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using System;
using UnityEngine;

public enum ItemCategory
{
    USABLE,
    WEAPON,
    JUNK
}

public abstract class Item : ScriptableObject
{
    public ItemCategory itemCategory;
    public string itemDescription;
    private Guid itemID;

    [Header("- Item Information -")] public string itemName;

    public GameObject itemPrefab;
    public int itemPrice;

    [Header("- Item Objects -")] public Sprite itemSprite;

    [Header("- Item Values -")] public int itemValue;

    public int itemWeight;

    public abstract void Use();

    public Guid GetID()
    {
        if (itemID.ToString() == "00000000-0000-0000-0000-000000000000")
            itemID = Guid.NewGuid();

        return itemID;
    }
}