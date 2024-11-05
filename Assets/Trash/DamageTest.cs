// ## - NK
using UnityEngine;
using FMODUnity;

public class DamageTest : MonoBehaviour
{
    [SerializeField]
    private float damage = 0f;

    private PlayerBehaviour playerBehaviour;

    //audio emitter variable
    private StudioEventEmitter emitter;

    private void Awake()
    {
        playerBehaviour = FindObjectOfType<PlayerBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerBehaviour.Damage(damage);
    }
}
