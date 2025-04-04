// ## - NK
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float attackStat;
    [SerializeField] protected float defenseStat;
    [SerializeField] protected float speedStat;
    [SerializeField] protected float hpStat;
    [SerializeField] protected float recoverStat;

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
    // A total defense of 100 results in a 50% damage reduction. Defense of 300 results in 75% damage reduction. -50 results in 200% damage
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
    protected SpriteRenderer spriteRenderer;
    protected Color color;

    [SerializeField] private StatsDisplay statsDisplay;

    // Obsolete. Invincibility is now handled differently.
    /*[SerializeField]
    protected bool isInvincible = false;*/

    [SerializeField]
    protected bool isFlammable = true;

    // This is to be used with the cushion upgrade on the player specifically.
    [HideInInspector]
    public float cushion = 0f;

    public bool capSpeedToTotalSpeed = true;
    [HideInInspector]
    public bool isOnFire = false;

    protected float tickDamage = 0f;

    protected float tickInterval = 0f;

    protected float tickTime = 0f;

    protected float timeToTick = 0f;

    protected virtual void Awake()
    {
        UpdateStats();
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    protected void Update()
    {
        Heal(totalRecovery * Time.deltaTime);
        if (isOnFire)
        {
            if (tickTime > 0f)
            {
                if (timeToTick > 0f)
                {
                    timeToTick -= Time.deltaTime;
                } else
                {
                    Damage(tickDamage);
                    timeToTick = tickInterval;
                }
                tickTime -= Time.deltaTime;
            } else
            {
                isOnFire = false;
            }
        }
    }

    public virtual void Damage(float amount)
    {
        Damage(amount, false);
    }

    public virtual void Damage(float amount, bool isContactDamage)
    {
        // This formula was taken from the Risk of Rain 2 Armor stat calculation: https://riskofrain2.fandom.com/wiki/Armor
        // It prevents damage from ever reaching 0
        
        // Obsolete. Invincibility is now handled differently.
        //if (!isInvincible)
        {
            float totalDamage = amount * (100 / (100 + totalDefense));
            if (isContactDamage)
            {
                totalDamage *= 1f - cushion;
            } else
            {
                totalDamage *= 100 / (100 + (cushion * 100));
            }
            health -= totalDamage;
            if (spriteRenderer != null)
            {
                StartCoroutine(DamageFlash());
            }
            if (totalDamage <= 0.01f)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.armadilloBlock, this.transform.position);
            }
            else
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyDamaged, this.transform.position);
            }
        }

        //Debug.Log("Took " + amount + " damage.");
        //Debug.Log("Health: " + health);
        if (health <= 0f)
        {
            Death();
        }
    }
    protected virtual IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color * 0.5f;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = color;
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

    public virtual void Ignite(float damage, float interval, float time, GameObject fireObject)
    {
        if (isFlammable && !isOnFire)
        {
            Debug.Log(name + " ignited!");
            isOnFire = true;
            tickDamage = damage;
            tickInterval = interval;
            tickTime = time;
            Instantiate(fireObject, transform);
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

        ShootAbility[] allShootAbilities = GetComponents<ShootAbility>();
        if (allShootAbilities != null)
        {
            foreach (ShootAbility ability in allShootAbilities)
            {
                ability.bulletKnockbackMultiplier = 1;
                ability.bulletSpeedMultiplier = 1;
                ability.cooldownMultiplier = 1;
            }
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
            //Debug.Log("Checking inventory...");
            foreach (InventorySlot slot in inventoryManager.inventory)
            {
                if (slot.item != null)
                {
                    //Debug.Log("Inventory slot " + slot + " has an item.");
                    if (slot.item.GetComponent<Upgrade>() != null)
                    {
                        slot.item.GetComponent<Upgrade>().ApplyUpgrade(slot.quantity);
                        //Debug.Log("Applied " + slot.item.gameObject.name + " " + slot.quantity + " time(s).");
                    }
                } //else { Debug.Log("Inventory slot " + slot + " does not have an item."); }
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
        //if (inventoryManager != null)
        //{
        //    Debug.Log("Speed: " + speedStat + " -> " + (totalSpeed / speedMultiplier) + " * " + speedMultiplier + " = " + totalSpeed);
        //    Debug.Log("Attack: " + attackStat + " -> " + (totalAttack / attackMultiplier) + " * " + attackMultiplier + " = " + totalAttack);
        //}
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
