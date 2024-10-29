using UnityEngine;

public class HealTest : MonoBehaviour
{
    [SerializeField]
    private float health = 0f;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.ChangeHealth(health);
    }
}
