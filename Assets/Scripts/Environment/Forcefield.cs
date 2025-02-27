using UnityEngine;

public class Forcefield : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private Collider2D forcefieldCollider;

    private SpriteRenderer sr;

    private bool isDeactivated = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDeactivated && ((playerLayer & (1 << collision.gameObject.layer)) != 0))
        {
            //FindAnyObjectByType<AudioParameterController>().IncrementIntensity(2);
            sr.enabled = true;
            Debug.Log("Player entered the beacon radius.");
            forcefieldCollider.enabled = true;
            FindObjectOfType<EnemyCounter>().SpawnBoss();
        }
    }

    public void Deactivate()
    {
        sr.enabled = false;
        forcefieldCollider.enabled = false;
        isDeactivated = true;
    }
}
