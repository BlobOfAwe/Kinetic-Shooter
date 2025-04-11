using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class Forcefield : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private Collider2D forcefieldCollider;
    //Commented out references to the sprite renderer since we are using vfx for the zone. -ZS
    //private SpriteRenderer sr;
    private VisualEffect beaconZone;
    [SerializeField]
    private GameObject beaconIndicator;
    private bool isDeactivated = false;

    private void Awake()
    {
        //sr = GetComponent<SpriteRenderer>();
        beaconZone = GetComponent<VisualEffect>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDeactivated && !beaconZone.enabled && ((playerLayer & (1 << collision.gameObject.layer)) != 0))
        {
            //FindAnyObjectByType<AudioParameterController>().IncrementIntensity(2);
            //sr.enabled = true;
            beaconZone.enabled = true;
            beaconIndicator.SetActive(false);
            //Debug.Log("Player entered the beacon radius.");
            forcefieldCollider.enabled = true;
            FindObjectOfType<EnemyCounter>().SpawnBoss();
        }
    }

    public void Deactivate()
    {
       // sr.enabled = false;
        beaconZone.enabled = false;
        forcefieldCollider.enabled = false;
        isDeactivated = true;
    }
}
