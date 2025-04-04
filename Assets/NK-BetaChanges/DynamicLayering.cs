using UnityEngine;

public class DynamicLayering : MonoBehaviour
{
    [SerializeField]
    private float centerYOffset = 0f;

    [SerializeField]
    private int sortingOrderBelow = -1;

    [SerializeField]
    private int sortingOrderAbove = 1;

    private Transform player;

    private SpriteRenderer sr;

    private void Awake()
    {
        player = FindObjectOfType<PlayerBehaviour>().transform;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player.position.y < transform.position.y + centerYOffset)
        {
            sr.sortingOrder = sortingOrderBelow;
        } else
        {
            sr.sortingOrder = sortingOrderAbove;
        }
    }
}
