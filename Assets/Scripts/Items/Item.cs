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
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.itemApproach, this.gameObject);
        emitter.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponentInChildren<InventoryManager>().AddItem(this);
        emitter.Stop();
        Destroy(gameObject);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.itemPickup, this.transform.position);
        Debug.Log("Picked up item");
    }
}
