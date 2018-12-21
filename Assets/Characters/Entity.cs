/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using System;
using System.Collections;
using System.Linq;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

/// <summary>
///     Different types that an entity can be.
///     The type of entity effects how some abilities work on it
/// </summary>
public enum EntityType
{
    CONSTRUCT,
    ELEMENTAL,
    UNDEAD
}

public enum Direction
{
    NORTH,
    NORTH_EAST,
    EAST,
    SOUTH_EAST,
    SOUTH,
    SOUTH_WEST,
    WEST,
    NORTH_WEST
}

/// <summary>
///     Parent entity class which all characters inherit from
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [Header("- Entity Object Variables -")]
    //public GameObject entityPrefab;
    public Ability[] entityAbilities;

    public string entityDescription;
    public DropsDictionary[] entityDrops;
    public Direction entityFacingDirection = Direction.EAST;

    [Header("- Entity Stats -")] public int entityHitPoints = 5;

    [Header("- Entity Toggable Variables -")]
    public bool entityIsInteractable = false;

    public bool entityIsOnGround;
    public float entityJumpStrength = 1.0f;
    public int entityMaxHitPoints = 5;
    public float entityMaxSpeed = 5.0f;

    [Header("- Entity Info -")] public string entityName;

    public float entitySpeed = 5.0f;
    public EntityType entityType;
    public bool isDead;
    public bool isPlayer = false;

    private void Start()
    {
        gameObject.name = entityName + $" ({(isPlayer ? "PLAYER" : "ENTITY")})";
        gameObject.tag = isPlayer ? "PLAYER" : "ENTITY";
    }

    private void Update()
    {
        foreach (var item in entityAbilities.Where(x => x.isActive)) item.Update();
    }

    /// <summary>
    ///     Method called when the entity is spawned.
    /// </summary>
    protected abstract void OnSpawn();

    /// <summary>
    ///     Method called when entity is killed. (Not destroyed!)
    /// </summary>
    /// <param name="entityKiller">Entity that was responsible. Null if suicide.</param>
    protected abstract void OnDeath(Entity entityKiller = null);

    /// <summary>
    ///     Method called when entity is damaged.
    /// </summary>
    /// <param name="entityDamager">Entity that was resnposible. Null if self inflicted.</param>
    protected abstract void OnHit(Entity entityDamager);

    /// <summary>
    ///     Method called when entity has been interacted with/
    /// </summary>
    /// <param name="entityInteracter">Entity that has interacted.</param>
    protected abstract void OnInteraction(Entity entityInteracter);

    /// <summary>
    ///     Uses the ability specified. Will return out of the method if the
    ///     ability is not available for the character.
    /// </summary>
    /// <param name="ability">Ability to use</param>
    public void UseAbility(Ability ability)
    {
        // Ensure we have the ability to use it
        if (!entityAbilities.Contains(ability)) return;
        ability.Use();
    }

    public Ability GetAbility(string name)
    {
        return entityAbilities.First(x => x.abilityName == name);
    }

    public void Hit(int damage, Entity damager)
    {
        entityHitPoints -= damage;
        if (entityHitPoints <= 0)
        {
            isDead = true;
            OnDeath(damager);
        }
        else
        {
            OnHit(damager);
        }
    }

    protected IEnumerator Blink(int amount = 3, float blinkDurationSeconds = 0.15f, float blinkAlpha = 0.3f,
        float fadeDelay = 0.0f, float blinkSpacing = 0.2f)
    {
        var sprite = gameObject.GetComponent<SpriteRenderer>();
        var originalColor = sprite.color;
        var fadedColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, blinkAlpha);
        for (var i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(fadeDelay);
            sprite.color = fadedColor;
            yield return new WaitForSeconds(blinkDurationSeconds);
            sprite.color = originalColor;
            yield return new WaitForSeconds(blinkSpacing);
        }
    }

    [Serializable]
    public class DropsDictionary : SerializableDictionaryBase<string, float>
    {
    }
}