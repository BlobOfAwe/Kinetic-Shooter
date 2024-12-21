using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private int score = 0;

    [SerializeField]
    private int menuSceneBuildIndex = 0;

    private static ScoreManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded");
        if (scene.buildIndex == menuSceneBuildIndex)
        {
            ResetScore();
        }
        if (FindObjectOfType<ScoreDisplay>() != null)
        {
            FindObjectOfType<ScoreDisplay>().UpdateScore(score);
            Debug.Log("Score updated to " + score + " for scene: " + scene.name);
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        Debug.Log("+" + points + " points!");
        if (FindObjectOfType<ScoreDisplay>() != null)
        {
            FindObjectOfType<ScoreDisplay>().UpdateScore(score);
        } else
        {
            Debug.Log("Score: " + score);
        }
    }

    public void ResetScore()
    {
        score = 0;
        if (FindObjectOfType<ScoreDisplay>() != null)
        {
            FindObjectOfType<ScoreDisplay>().UpdateScore(score);
        }
    }
}
