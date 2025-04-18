using Cinemachine;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

public class Beacon : MonoBehaviour
{
    [SerializeField]
    private Forcefield forcefield;
    public bool active;
    [SerializeField] float bossCamSize;
    private float startingCamSize;
    [SerializeField] float timeToFullZoomOut = 1f;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private GameObject winPanel;
    private InventoryManager playerInv;
    [HideInInspector]
    public bool levelIsFinished = false; // temporary\
    [SerializeField]
    private PlayerBehaviour player;

    //audio emitter variable
    private StudioEventEmitter emitter;

    // audio parameter controller script
    [SerializeField] AudioParameterController parameterController;

    [SerializeField]
    private GameObject winButton;

    public void Start()
    {
        //creates an audio emitter and plays event
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.beaconLoop, this.gameObject);
        emitter.Play();

        player = FindAnyObjectByType<PlayerBehaviour>();
        playerInv = player.GetComponentInChildren<InventoryManager>();
        vCam = FindAnyObjectByType<CinemachineVirtualCamera>();
        startingCamSize = vCam.m_Lens.OrthographicSize;
    }

    public void Activate()
    {
        active = true;

        // FindAnyObjectByType<AudioParameterController>().IncrementIntensity(-1);

        StartCoroutine(ZoomOut());

        // Stop the enemy spawner and find and destroy all loaded enemies - JV
        FindAnyObjectByType<EnemySpawner>().active = false;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            // Splitters behave uniquely when dying. Destroy them directly to avoid problematic behaviour
            if (enemies[i].GetType() == typeof(LeaderSplitterEnemy))
            {
                Destroy(enemies[i].gameObject);
            }
            else
            {
                enemies[i].Death();
            }

        }
        
        Debug.Log("Beacon activated!");
        forcefield.gameObject.SetActive(true);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // temporary
        if (levelIsFinished)
        {
            StartCoroutine(WinGame());
            player.TeleportAnim();
            //WinGame();
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
    protected virtual IEnumerator WinGame()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.difficultyCoefficient++;

        // If the current level is not the last level, load the next level
        if (GameManager.currentLevel < GameManager.sceneIndexForLevel.Length - 1)
        {
            playerInv.PreserveInventory();
            GameManager.currentLevel++;
            SceneManager.LoadSceneAsync(GameManager.sceneIndexForLevel[GameManager.currentLevel]);
        }
        // Otherwise, enable the win panel
        else
        {
            winPanel.SetActive(true);
            parameterController.EndingWin();
            FindObjectOfType<CursorManager>().isEndScreen = true;
            EventSystem.current.SetSelectedGameObject(winButton);
        }
    }
}
