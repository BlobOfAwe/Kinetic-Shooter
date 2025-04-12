// ## - GZ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    public float musicVolume = 1;
    public float ambienceVolume = 1;
    public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus SFXBus;

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    private EventInstance menuMusic;
    private EventInstance industrialMusic;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    public static int songSelection = 0;

    public static AudioManager instance {  get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        else
        {
            instance = this;
        }

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    private void Start()
    {
        InitiliazeAmbience(FMODEvents.instance.allAmbience);
        InitiliazeMusic(FMODEvents.instance.allMusic);

        /*
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        // Main Menu
        if (sceneName == "MainMenu")
        {
            MusicSelection musicSelection = MusicSelection.MENU;
            SetMusicSelection(musicSelection);
        }
        // Industrial Level
        else if (sceneName == "MAIN")
        {
            MusicSelection musicSelection = MusicSelection.INDUSTRIAL;
            SetMusicSelection(musicSelection);
        }
        // Jungle Level
        else if (sceneName == "MAIN_2")
        {
            MusicSelection musicSelection = MusicSelection.JUNGLE;
            SetMusicSelection(musicSelection);
        }
        // Lava Level
        else if (sceneName == "MAIN_3")
        {
            MusicSelection musicSelection = MusicSelection.LAVA;
            SetMusicSelection(musicSelection);
        }
        
        
        // Main Menu
        if (songSelection == 0)
        {
            InitiliazeAmbience(FMODEvents.instance.mainMenuAmbience);
            InitiliazeMusic(FMODEvents.instance.mainMenuMusic);
        }
        // Industrial Level
        else if (songSelection == 1)
        {
            InitiliazeAmbience(FMODEvents.instance.industrialAmbience);
            InitiliazeMusic(FMODEvents.instance.industrialMusic);
        }
        // Jungle Level
        else if (songSelection == 2)
        {
            InitiliazeAmbience(FMODEvents.instance.jungleAmbience);
            InitiliazeMusic(FMODEvents.instance.jungleMusic);
        }
        // Lava Level
        else if (songSelection == 3)
        {
            InitiliazeAmbience(FMODEvents.instance.lavaAmbience);
            InitiliazeMusic(FMODEvents.instance.lavaMusic);
        }
        */
    }

    private void Update()
    {

        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        // Main Menu
        if (sceneName == "MainMenu")
        {
            MusicSelection musicSelection = MusicSelection.MENU;
            SetMusicSelection(musicSelection);
            SetAmbienceSelection(musicSelection);
        }
        // Industrial Level
        else if (sceneName == "MAIN")
        {
            MusicSelection musicSelection = MusicSelection.INDUSTRIAL;
            SetMusicSelection(musicSelection);
            SetAmbienceSelection(musicSelection);
        }
        // Jungle Level
        else if (sceneName == "MAIN_2")
        {
            MusicSelection musicSelection = MusicSelection.JUNGLE;
            SetMusicSelection(musicSelection);
            SetAmbienceSelection(musicSelection);
        }
        // Lava Level
        else if (sceneName == "MAIN_3")
        {
            MusicSelection musicSelection = MusicSelection.LAVA;
            SetMusicSelection(musicSelection);
            SetAmbienceSelection(musicSelection);
        }
        // Cutscene
        else if (sceneName == "cutscene")
        {
            MusicSelection musicSelection = MusicSelection.CUTSCENE;
            SetMusicSelection(musicSelection);
            SetAmbienceSelection(musicSelection);
        }

        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        SFXBus.setVolume(SFXVolume);

    }

    private void InitiliazeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    private void InitiliazeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetMusicIntensity(string parameterName, float parameterValue)
    {
        musicEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void SetMusicSelection(MusicSelection musicSelection)
    {
        musicEventInstance.setParameterByName("level", (float) musicSelection);
    }

    public void SetAmbienceSelection(MusicSelection musicSelection)
    {
        ambienceEventInstance.setParameterByName("level", (float)musicSelection);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
