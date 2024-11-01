using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{


    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private int gameOverScene = 0;

    public float health;

    private void Awake()
    {
        health = maxHealth;
    }

    /*
     * Damage() and Heal() could potentially be the same function, and could heal or damage based on whether "amount" is positive or negative, but in the
     * future, the effects of taking damage and healing may be different. For instance, the player may have i-frames after damage, but not after healing,
     * so it may be best to have them in separate functions. However, it would also be an option to have an if statement that does something different
     * depending on whether or not "amount" is negative.
     */

    public void Damage(float amount)
    {
        health -= amount;
        Debug.Log("Took " + amount + " damage.");
        Debug.Log("Health: " + health);
        if (health <= 0f)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene(gameOverScene);
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        Debug.Log("Healed " + amount + " health.");
        Debug.Log("Health: " + health);
    }
}
