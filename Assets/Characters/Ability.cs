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

public abstract class Ability : ScriptableObject
{
	[Header("- Ability Information -")]
	public string abilityName;
	public string abilityDescription;
	public string abilityInstructions;
	public float cooldownPeriod;
	[Tooltip("The amount the ability will do.\nFor example:\n  It will heal for abilityValue.\n  It will damage for abilityValue.")]
	public float abilityValue;
	
	[HideInInspector]
	public bool isActive = false;

	protected float cooldownSecondsRemaining;

	[Header("- Ability Variables -")]
	[Tooltip("The object that will be spawned if appropiate.")]
	public GameObject abilityPrefab;
	public GameObject abilityOwner;
	public Animation abilityAnimation;
	//public Particle abilityParticle;

	public abstract void Start();
	public abstract void Update();
	public abstract void Use();
}
