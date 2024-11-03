using UnityEngine;
using UnityEngine.SceneManagement;

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

    public virtual void Damage(float amount)
    {
        health -= amount;
        Debug.Log("Took " + amount + " damage.");
        Debug.Log("Health: " + health);
        if (health <= 0f)
        {
            Debug.Log("Game Over");
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
        Debug.Log("Healed " + amount + " health.");
        Debug.Log("Health: " + health);
    }
    /*public void ChangeHealth(float amount)
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
    }*/

    // JV - Changed to Abstract function to force derivative classes to override the function with unique behaviour
    protected abstract void Death();

    public virtual void UseAbility(Ability ability)
    {
        if (ability.available)
        {
            ability.OnActivate();
        }
    }
}
