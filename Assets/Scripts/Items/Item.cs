// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public abstract class Item : MonoBehaviour
{
    public Sprite sprite;
    public string title;
    public string description;

    //audio emitter variable
    private StudioEventEmitter emitter;

    protected virtual void Awake()
    {
        //creates an audio emitter and plays event
        // COMMENTED OUT LINES THAT HAVE TO DO WITH AN AUDIO EMITTER BECAUSE IT CAUSES ISSUES - NK
        //emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.itemApproach, this.gameObject);
        //emitter.Play();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponentInChildren<InventoryManager>().AddItem(this);
        //Destroy(gameObject);
        gameObject.SetActive(false); // The item should be disabled, not destroyed. Otherwise, the item that goes into the inventory will be missing.
        //emitter.Stop();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.itemPickup, this.transform.position);
        Debug.Log("Picked up item");
    }
}
