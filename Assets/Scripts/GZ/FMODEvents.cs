// ## - GZ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player Movement")]
    [field: SerializeField] public EventReference playerMovementSound { get; private set; }

    [field: Header("Player Gunshot")]
    [field: SerializeField] public EventReference playerGunshot { get; private set; }

    [field: Header("Item SFX")]
    [field: SerializeField] public EventReference itemIdleSound { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference windAmbience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference menuMusic { get; private set; }

    public static FMODEvents instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than on FMOD Events instance in the scene");
        }
        instance = this;
    }

}
