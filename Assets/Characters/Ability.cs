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
    [Header("- Ability Information -")] public string abilityName;

    public GameObject abilityOwner;
    public string abilityDescription;
    public string abilityInstructions;

    [Header("- Ability Variables -")] [Tooltip("The object that will be spawned if appropiate.")]
    public GameObject abilityPrefab;
    public Animation abilityAnimation;

    [Tooltip(
        "The amount the ability will do.\nFor example:\n  It will heal for abilityValue.\n  It will damage for abilityValue.")]
    public float abilityValue;

    public float cooldownPeriod;

    protected float cooldownSecondsRemaining;

    [HideInInspector] public bool isActive = false;
    //public Particle abilityParticle;

    public abstract void Start();
    public abstract void Update();
    public abstract void Use();

    public static void NewAbilityNotification(Ability ability, string optional = "")
    {
        InputManager.GetInstance()._onNotificationClose.RemoveAllListeners();
        CanvasScript.SetNotificationBoxTitle($"<b>New ability\nunlocked!</b>");
		CanvasScript.SetNotificationBoxText($"<b>{ability.abilityName}</b>\n\n{ability.abilityDescription}\n\n{ability.abilityInstructions}");
		CanvasScript.SetNotificationBoxCloseText("Got it!");
		CanvasScript.ShowNotificationBox();
    }
}