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
    public Image additionalCooldown;

    private void Update()
    {
       UpdateCooldownDisplay(primaryCooldown, player.primary);
        UpdateCooldownDisplay(secondaryCooldown, player.secondary);
        UpdateCooldownDisplay(utilityCooldown, player.utility);
        UpdateCooldownDisplay(additionalCooldown, player.additional);
    }

    private void UpdateCooldownDisplay(Image cooldownImage, Ability ability)
    {
        if (ability == null || cooldownImage == null) return;

        float ratio = Mathf.Clamp01(ability.currentCooldown / ability.cooldown);
        float fillAmount = 1 - Mathf.InverseLerp(0, ability.cooldown, ability.currentCooldown);

        cooldownImage.fillAmount = fillAmount;
        cooldownImage.color = fillAmount < 1 ?
            new Color(0.8f, 0.8f, 0.8f, 0.5f) :
            new Color(1, 1, 1, 0.2f);
    }
}