using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth;

    public float health;

    public Ability primary, secondary, utility, additional;

    private void Awake()
    {
        health = maxHealth;
    }

    public void ChangeHealth(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        Debug.Log("Health changed by " + amount + ".");
        Debug.Log("Health: " + health);
        if (health <= 0f)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        Debug.Log(gameObject.name + " died.");
    }

    public virtual void UseAbility(Ability ability)
    {
        if (ability.available)
        {
            ability.OnActivate();
        }
    }
}