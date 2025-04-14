using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    public PlayerBehaviour player;
    public Image primaryCooldown;
    public Image secondaryCooldown;
    public Image utilityCooldown;
    //public Image additionalCooldown; - Removed by Nathaniel Klassen
    // Following variables and all to do with them added by Nathaniel Klassen
    public Image primaryButtonPrompt;
    public Image secondaryButtonPrompt;
    public Image utilityButtonPrompt;

    // Added by Nathaniel Klassen
    [SerializeField]
    private Color cooldownFullColor = Color.white;
    [SerializeField]
    private Color cooldownWaitColor = Color.white;
    [SerializeField]
    private Color buttonFullColor = Color.white;
    [SerializeField]
    private Color buttonWaitColor = Color.white;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerBehaviour>();
    }

    private void Update()
    {
        UpdateCooldownDisplay(primaryCooldown, primaryButtonPrompt, player.primary);
        UpdateCooldownDisplay(secondaryCooldown, secondaryButtonPrompt, player.secondary);
        UpdateCooldownDisplay(utilityCooldown, utilityButtonPrompt, player.utility);
        //UpdateCooldownDisplay(additionalCooldown, player.additional); - Removed by Nathaniel Klassen
    }

    private void UpdateCooldownDisplay(Image cooldownImage, Image buttonPrompt, Ability ability)
    {
        if (ability == null || cooldownImage == null) return;

        float ratio = Mathf.Clamp01(ability.currentCooldown / ability.cooldown);
        float fillAmount = 1 - Mathf.InverseLerp(0, ability.cooldown, ability.currentCooldown);

        // Changed to use color variables set in inspector instead of hard-coded colors. - NK
        cooldownImage.fillAmount = fillAmount;
        cooldownImage.color = fillAmount < 1 ?
            cooldownWaitColor :
            cooldownFullColor;

        buttonPrompt.fillAmount = fillAmount;
        buttonPrompt.color = fillAmount < 1 ?
            buttonWaitColor :
            buttonFullColor;
    }
}