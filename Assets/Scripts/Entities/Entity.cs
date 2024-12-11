// ## - NK
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float attackStat;
    [SerializeField] private float defenseStat;
    [SerializeField] private float speedStat;
    [SerializeField] private float hpStat;
    [SerializeField] private float recoverStat;

    [Header("Buff/Debuff Lists")]
    public List<Buff> attackBuffs;
    public List<Buff> defenseBuffs;
    public List<Buff> speedBuffs;
    public List<Buff> hpBuffs;
    public List<Buff> recoverBuffs;

    [Header("Final Values")]
    public float maxHealth;
    public float health;
    public float totalAttack;
    // The equation for damage is: damage * (100/(100+totalDefense)). This will never reach 0.
    // A total defense of 100 results in a 50% damage reduction. Defense of 300 results in 75% damage reduction.
    public float totalDefense;  
    public float totalSpeed;
    public float totalRecovery;

    // Keeps track of multiplicative buffs
    [Header("Buff Multipliers")]
    public float healthMultiplier;
    public float attackMultiplier;
    public float defenseMultiplier;
    public float speedMultiplier;
    public float recoveryMultiplier;

    [Header("Abilities")]
    public Ability primary;
    public Ability secondary;
    public Ability utility;
    public Ability additional;

    [Header("Component References")]
    [HideInInspector] public Rigidbody2D rb;
    protected InventoryManager inventoryManager;

    [SerializeField] private StatsDisplay statsDisplay;

    [SerializeField]
    protected bool isInvincible = false;


    protected void Awake()
    {
        UpdateStats();
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
    }

    protected void Update()
    {
        Heal(totalRecovery * Time.deltaTime);
    }

    public virtual void Damage(float amount)
    {
        // This formula was taken from the Risk of Rain 2 Armor stat calculation: https://riskofrain2.fandom.com/wiki/Armor
        // It prevents damage from ever reaching 0
        if (!isInvincible)
        {
            health -= amount * (100 / (100 + totalDefense));
        }
        //Debug.Log("Took " + amount + " damage.");
        //Debug.Log("Health: " + health);
        if (health <= 0f)
        {
            Death();
        }
    }
    public virtual void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public abstract void Death();
    
    // To be called whenever a change ocurrs to any of the buff lists
    public void UpdateStats()
    {
        float currentHPPercentage = health / maxHealth;

        // Reset TestShootBullet if applicable
        TestShootBullet testShootBullet = GetComponent<TestShootBullet>();
        if (testShootBullet != null) 
        {
            testShootBullet.bulletKnockbackMultiplier = 1;
            testShootBullet.bulletSpeedMultiplier = 1;
        }

        // Reset StandardPrimaryFire if applicable
        StandardPrimaryFire standardPrimaryFire = GetComponent<StandardPrimaryFire>();
        if (standardPrimaryFire != null)
        {
            standardPrimaryFire.bulletKnockbackMultiplier = 1;
            standardPrimaryFire.bulletSpeedMultiplier = 1;
        }

        maxHealth = hpStat;
        totalAttack = attackStat;
        totalDefense = defenseStat;
        totalSpeed = speedStat;
        totalRecovery = recoverStat;

        attackMultiplier = 1;
        defenseMultiplier = 1;
        speedMultiplier = 1;
        healthMultiplier = 1;
        recoveryMultiplier = 1;

        // UPGRADES
        if (inventoryManager != null)
        {
            Debug.Log("Checking inventory...");
            foreach (InventorySlot slot in inventoryManager.inventory)
            {
                if (slot.item != null)
                {
                    Debug.Log("Inventory slot " + slot + " has an item.");
                    if (slot.item.GetComponent<Upgrade>() != null)
                    {
                        slot.item.GetComponent<Upgrade>().ApplyUpgrade(slot.quantity);
                        Debug.Log("Applied " + slot.item.gameObject.name + " " + slot.quantity + " time(s).");
                    }
                } else { Debug.Log("Inventory slot " + slot + " does not have an item."); }
            }
        }

        // ATTACK
        foreach (Buff buff in attackBuffs) 
        {
            if (buff.modification == Buff.modificationType.Additive) { totalAttack += buff.value; } 
            else if (buff.modification == Buff.modificationType.Multiplicative) { attackMultiplier *= buff.value; }
        }
        totalAttack *= attackMultiplier;

        // DEFENSE
        foreach (Buff buff in defenseBuffs) 
        {
            if (buff.modification == Buff.modificationType.Additive) { totalDefense += buff.value; }
            else if (buff.modification == Buff.modificationType.Multiplicative) { defenseMultiplier *= buff.value; }
        }
        totalDefense *= defenseMultiplier;

        if (totalDefense < -99)
        {
            Debug.LogError("Total defense cannot exceed -99 in the negative range. Set total defense to -99.");
            totalDefense = -99;
        }

        // SPEED
        foreach (Buff buff in speedBuffs) 
        {
            if (buff.modification == Buff.modificationType.Additive) { totalSpeed += buff.value; }
            else if (buff.modification == Buff.modificationType.Multiplicative) { speedMultiplier *= buff.value; }
        }
        totalSpeed *= speedMultiplier;

        // HP
        foreach (Buff buff in hpBuffs) 
        {
            if (buff.modification == Buff.modificationType.Additive) { maxHealth += buff.value; }
            else if (buff.modification == Buff.modificationType.Multiplicative) { healthMultiplier *= buff.value; }
        }
        maxHealth *= healthMultiplier;
        health = maxHealth * currentHPPercentage;

        // RECOVERY
        foreach (Buff buff in recoverBuffs) 
        {
            if (buff.modification == Buff.modificationType.Additive) { totalRecovery += buff.value; }
            else if (buff.modification == Buff.modificationType.Multiplicative) { recoveryMultiplier *= buff.value; }
        }
        totalRecovery *= recoveryMultiplier;

        // DEBUG
        if (inventoryManager != null)
        {
            Debug.Log("Speed: " + speedStat + " -> " + (totalSpeed / speedMultiplier) + " * " + speedMultiplier + " = " + totalSpeed);
            Debug.Log("Attack: " + attackStat + " -> " + (totalAttack / attackMultiplier) + " * " + attackMultiplier + " = " + totalAttack);
        }
        if (statsDisplay != null)
        {
            statsDisplay.UpdateDisplay();
        }
    }

    public virtual void UseAbility(Ability ability)
    {
        if (ability.available && !GameManager.paused)
        {
            ability.OnActivate();
        }
    }

    // Applies a buff or effect to this GameObject
    public void ApplyBuff(Buff buff)
    {
        switch (buff.buffType)
        {
            case Buff.buffCategory.ATTACK_BUFF:
                attackBuffs.Add(buff);
                break;
            case Buff.buffCategory.DEFENSE_BUFF:
                defenseBuffs.Add(buff);
                break;
            case Buff.buffCategory.SPEED_BUFF:
                speedBuffs.Add(buff);
                break;
            case Buff.buffCategory.HP_BUFF:
                hpBuffs.Add(buff);
                break;
            case Buff.buffCategory.RECOVER_BUFF:
                recoverBuffs.Add(buff);
                break;
        }

        if (buff.duration > 0)
        {
            StartCoroutine(BuffDuration(buff));
        }

        UpdateStats();
    }

    // Applies a buff or effect to this GameObject
    public void RemoveBuff(Buff buff)
    {
        switch (buff.buffType)
        {
            case Buff.buffCategory.ATTACK_BUFF:
                attackBuffs.Remove(buff);
                break;
            case Buff.buffCategory.DEFENSE_BUFF:
                defenseBuffs.Remove(buff);
                break;
            case Buff.buffCategory.SPEED_BUFF:
                speedBuffs.Remove(buff);
                break;
            case Buff.buffCategory.HP_BUFF:
                hpBuffs.Remove(buff);
                break;
            case Buff.buffCategory.RECOVER_BUFF:
                recoverBuffs.Remove(buff);
                break;
        }

        UpdateStats();
    }

    IEnumerator BuffDuration(Buff buff)
    {
        yield return new WaitForSeconds(buff.duration);
        try { RemoveBuff(buff); Debug.Log("Removed buff."); }
        catch { Debug.LogWarning("Attempted to remove non-existent buff."); }
    }
}
