using UnityEngine;

public class Forcefield : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer & (1 << collision.gameObject.layer)) != 0)
        {
            sr.enabled = true;
            Debug.Log("Player entered the beacon radius.");
        }
    }
}
