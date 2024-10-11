using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("SFX 1")]
    [field: SerializeField] public EventReference meowMoment { get; private set; }

    [field: Header("SFX 2")]
    [field: SerializeField] public EventReference chickenMoment { get; private set; }

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
