/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using UnityEngine;

/// <summary>
/// Class used to show how an entity can be setup.
/// Inherits from <param=Entity>Entity</param>
/// </summary>
public class TemplateEnemyEntity : Entity
{
    protected override void OnSpawn()
    {
        // Do stuff when entity is spawned
    }

    protected override void OnDeath(Entity entityKiller = null)
    {
        // Do stuff when killed (NOT DESTROYED!)
    }

    protected override void OnHit(Entity entityDamager)
    {
        // Do stuff when hit by entityDamager
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        // Do stuff when interacted with by entityInteracter
    }

    // Necessary to execute 'OnSpawn()'
    void Start()
        => OnSpawn();
    
}
