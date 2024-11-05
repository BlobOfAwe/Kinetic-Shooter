// ## - ZS
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    //Basic script that handles the pickups, made only for testing purposes
    public Pickup pickupData;
    public Sidebar sidebar;

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player"))
        {
            CollectPickup();
        }
    }

    private void CollectPickup()
    {
        sidebar.AddPickup(pickupData);
        Destroy(gameObject);
    }
}