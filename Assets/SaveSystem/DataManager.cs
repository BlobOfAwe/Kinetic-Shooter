using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public GameData gameData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Destroying duplicate DataManager.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        if (gameData != null)
        {
            SaveSystem.SaveData(gameData);
        } else
        {
            Debug.Log("No data was found. Setting to default values.");
            NewGame();
        }
    }

    public void LoadGame()
    {
        if (SaveSystem.LoadData() != null)
        {
            gameData = SaveSystem.LoadData();
            if (LogBookManager.Instance != null)
            {
                UpdateLogBook();
            }
        }
    }

    public void DeleteGame()
    {
        SaveSystem.DeleteData();
        gameData = new GameData();
    }

    private void UpdateLogBook()
    {
        //*if (gameData.enemyTypeKills >= required)
        //{
        //    LogBookManager.Instance.UnlockEntry("Name of Log Book Entry");
        //}*/

        // Below are placeholder conditions because log book is very unfinished currently.
        //if (gameData.bouncerKills >= 1)
        //{
        //    LogBookManager.Instance.UnlockEntry("TheGoodOlScoth");
        //}
        //if (gameData.totalKills >= 10)
        //{
        //    LogBookManager.Instance.UnlockEntry("Why");
        //}
        //if (gameData.splitterKills >= 3)
        //{
        //    LogBookManager.Instance.UnlockEntry("SameOlRye");
        //}
    }
}
