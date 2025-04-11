using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootAbility : Ability
{
    // Added multipliers for bullet speed, knockback, and damage to be manipulated with upgrades. - NK
    public float bulletSpeedMultiplier = 1f;
    public float bulletKnockbackMultiplier = 1f;
    public float bulletDamageMultiplier = 1f;
    public float upgradeTriggerRate = 1f;

    [HideInInspector]
    public GameObject[] bullets; // Changed to public so it can be used with upgrade behaviour. - NK
}