using UnityEngine;
using FMODUnity;

public class DamageTest : MonoBehaviour
{
    [SerializeField]
    private float damage = 0f;

    private PlayerDamage playerDamage;

    //audio emitter variable
    private StudioEventEmitter emitter;

    private void Awake()
    {
        playerDamage = FindObjectOfType<PlayerDamage>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerDamage.Damage(damage);
    }
}
