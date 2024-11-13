using FMODUnity;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField]
    private Forcefield forcefield;
    public bool active;

    //audio emitter variable
    private StudioEventEmitter emitter;

    public void Awake()
    {
        //creates an audio emitter and plays event
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.beaconLoop, this.gameObject);
        emitter.Play();
    }

    public void Activate()
    {
        active = true;

        // Stop the enemy spawner and find and destroy all loaded enemies - JV
        FindAnyObjectByType<EnemySpawner>().active = false;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies) { enemy.gameObject.SetActive(false); }
        
        Debug.Log("Beacon activated!");
        forcefield.gameObject.SetActive(true);
        
    }
}
