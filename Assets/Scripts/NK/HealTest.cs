using UnityEngine;

public class HealTest : MonoBehaviour
{
    [SerializeField]
    private float health = 0f;

    private PlayerDamage playerDamage;

    private void Awake()
    {
        playerDamage = FindObjectOfType<PlayerDamage>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerDamage.Heal(health);
    }
}
