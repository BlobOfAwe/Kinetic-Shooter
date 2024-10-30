using UnityEngine;
using FMODUnity;

public class DamageTest : MonoBehaviour
{
    [SerializeField]
    private float damage = 0f;

    private Player player;

    //audio emitter variable
    private StudioEventEmitter emitter;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.ChangeHealth(-damage);
    }
}
