using UnityEngine;
using FMODUnity;

public class HealTest : MonoBehaviour
{
    [SerializeField]
    private float health = 0f;

    [SerializeField] PlayerDamage playerDamage;

    //audio emitter variable
    private StudioEventEmitter emitter;

    private void Awake()
    {
        playerDamage = FindObjectOfType<PlayerDamage>();
        //creates an audio emitter and plays event
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.itemIdleSound, this.gameObject);
        emitter.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerDamage.Heal(health);
        //stops emitter when collected
        emitter.Stop();
    }
}
