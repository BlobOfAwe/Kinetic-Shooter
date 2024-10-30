using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(StudioEventEmitter))]

public class ItemAudioEmitter : MonoBehaviour
{
    private StudioEventEmitter emitter;

    private void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.itemIdleSound, this.gameObject);
        emitter.Play();
    }
}
