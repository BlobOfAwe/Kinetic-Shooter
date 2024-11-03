using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite sprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponentInChildren<InventoryManager>().AddItem(this);
        Destroy(gameObject);
        Debug.Log("Picked up item");
    }
}
