// ## - NK
using UnityEngine;

public abstract class Upgrade : Item
{
    protected PlayerBehaviour player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerBehaviour>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        ApplyUpgrade();
        base.OnTriggerEnter2D(collision);
    }

    protected virtual void ApplyUpgrade()
    {
        Debug.Log("Upgrade applied.");
    }
}
