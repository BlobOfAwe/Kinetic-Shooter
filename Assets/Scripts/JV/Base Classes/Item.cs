using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite sprite;
    public string title;
    public string description;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponentInChildren<InventoryManager>().AddItem(this);
        Destroy(gameObject);
        Debug.Log("Picked up item");
    }
}
