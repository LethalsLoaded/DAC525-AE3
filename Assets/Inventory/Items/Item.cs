/*
 * This file has been created by Daniel Wesolowski for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Daniel Wesolowski unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using UnityEngine;

public enum ItemCategory
{
	USABLE,
	WEAPON,
	JUNK
}
public abstract class Item : ScriptableObject 
{
	[Header("- Item Information -")]
	public string itemName;
	public string itemDescription;
	public ItemCategory itemCategory;
	[Header("- Item Values -")]
	public int itemValue;
	public int itemPrice;
	public int itemWeight;
	[Header("- Item Objects -")]
	public Sprite itemSprite;
	public GameObject itemPrefab;
	


	public abstract void Use();
	
}
