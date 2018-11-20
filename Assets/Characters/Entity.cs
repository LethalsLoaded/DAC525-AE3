/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using System.Linq;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

/// <summary>
/// Different types that an entity can be.
/// The type of entity effects how some abilities work on it
/// </summary>
public enum EntityType
{
	CONSTRUCT,
	ELEMENTAL,
	UNDEAD
}

/// <summary>
/// Parent entity class which all characters inherit from
/// </summary>
public abstract class Entity : MonoBehaviour
{
	[System.Serializable]
	public class DropsDictionary : SerializableDictionaryBase<string, float> { };

	[Header("- Entity Info -")]
	public string entityName;
	public string entityDescription;
	public EntityType entityType;

	[Header("- Entity Stats -")]
	public int entityHitPoints = 10;
	public int entityMaxHitPoints = 10;
	public float entitySpeed = 5.0f;

	[Header("- Entity Toggable Variables -")]
	public bool entityIsInteractable = false;
	public bool isPlayer = false;
	public bool isDead = false;

	[Header("- Entity Object Variables -")]
	public GameObject entityPrefab;
	public Ability[] entityAbilities;
	public DropsDictionary[] entityDrops;

	void Start()
	{
		gameObject.name = entityName + $" ({(isPlayer ? "PLAYER" : "ENTITY")})";
		gameObject.tag =  isPlayer ? "PLAYER" : "ENTITY";
	}

	/// <summary>
	/// Method called when the entity is spawned.
	/// </summary>
	protected abstract void OnSpawn();

	/// <summary>
	/// Method called when entity is killed. (Not destroyed!)
	/// </summary>
	/// <param name="entityKiller">Entity that was responsible. Null if suicide.</param>
	protected abstract void OnDeath(Entity entityKiller = null);

	/// <summary>
	/// Method called when entity is damaged.
	/// </summary>
	/// <param name="entityDamager">Entity that was resnposible. Null if self inflicted.</param>
	protected abstract void OnHit(Entity entityDamager);

	/// <summary>
	/// Method called when entity has been interacted with/
	/// </summary>
	/// <param name="entityInteracter">Entity that has interacted.</param>
	protected abstract void OnInteraction(Entity entityInteracter);

	/// <summary>
	/// Uses the ability specified. Will return out of the method if the
	/// ability is not available for the character.
	/// </summary>
	/// <param name="ability">Ability to use</param>
	void UseAbility(Ability ability)
	{
		// Ensure we have the ability to use it
		if(!entityAbilities.Contains(ability)) return;
	}

}
