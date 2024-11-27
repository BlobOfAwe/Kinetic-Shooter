// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using static WarCallBuff;

public abstract class Item : MonoBehaviour
{
    public Sprite sprite;
    public string title;
    public string description;

    //audio emitter variable
    protected StudioEventEmitter emitter;

    protected void Start()
    {
        //creates an audio emitter and plays event
        //emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.itemApproach, this.gameObject);
        //emitter.Play();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            collision.GetComponentInChildren<InventoryManager>().AddItem(this);

            //emitter.Stop();
            gameObject.SetActive(false); // The item should be disabled, not destroyed. Otherwise, the item that goes into the inventory will be missing.


            AudioManager.instance.PlayOneShot(FMODEvents.instance.itemPickup, this.transform.position);
            Debug.Log("Picked up item");
        }
    }
}
