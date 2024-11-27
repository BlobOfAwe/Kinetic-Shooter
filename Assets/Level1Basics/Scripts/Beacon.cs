using Cinemachine;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beacon : MonoBehaviour
{
    [SerializeField]
    private Forcefield forcefield;
    public bool active;
    [SerializeField] float bossCamSize;
    private float startingCamSize;
    [SerializeField] float timeToFullZoomOut = 1f;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private string nextLevelScene = "";
    [HideInInspector]
    public bool levelIsFinished = false; // temporary

    //audio emitter variable
    private StudioEventEmitter emitter;

    public void Awake()
    {
        //creates an audio emitter and plays event
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.beaconLoop, this.gameObject);
        emitter.Play();
    }

    private void Start()
    {

        vCam = FindAnyObjectByType<CinemachineVirtualCamera>();
        startingCamSize = vCam.m_Lens.OrthographicSize;
        Debug.LogWarning("Hi I started");
    }

    public void Activate()
    {
        active = true;

        FindAnyObjectByType<AudioParameterController>().IncrementIntensity(-1);

        StartCoroutine(ZoomOut());

        // Stop the enemy spawner and find and destroy all loaded enemies - JV
        FindAnyObjectByType<EnemySpawner>().active = false;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies) { enemy.gameObject.SetActive(false); }
        
        Debug.Log("Beacon activated!");
        forcefield.gameObject.SetActive(true);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // temporary
        if (levelIsFinished)
        {
            SceneManager.LoadScene(nextLevelScene);
        }
    }

    IEnumerator ZoomOut()
    {
        var deltaZoom = bossCamSize - startingCamSize;
        while (vCam.m_Lens.OrthographicSize < bossCamSize)
        {
            vCam.m_Lens.OrthographicSize += deltaZoom * Time.deltaTime / timeToFullZoomOut;
            yield return new WaitForEndOfFrame();
        }
        vCam.m_Lens.OrthographicSize = bossCamSize;
    }
}
