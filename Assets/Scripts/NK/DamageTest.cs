using UnityEngine;

public class DamageTest : MonoBehaviour
{
    [SerializeField]
    private float damage = 0f;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.ChangeHealth(-damage);
    }
}
