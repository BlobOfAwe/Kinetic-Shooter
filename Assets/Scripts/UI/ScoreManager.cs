using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private int score = 0;

    [SerializeField]
    private int menuSceneBuildIndex = 0;

    [SerializeField] 
    private ScoreDisplay scoreDisplay; //Added By ZS to reference ScoreDisplay

    [SerializeField] 
    private ScoreDisplay deathScoreDisplay; //Added By ZS to reference DeathScoreDisplay

    [SerializeField]
    private ScoreDisplay winScoreDisplay; //Added By ZS to reference WinScoreDisplay

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
        UpdateScoreDisplays();
        // if (FindObjectOfType<ScoreDisplay>() != null)
        // {
        //     FindObjectOfType<ScoreDisplay>().UpdateScore(score);
        //     Debug.Log("Score updated to " + score + " for scene: " + scene.name);
        // }
    }

    public void AddPoints(int points)
    {
        score += points;
        Debug.Log("+" + points + " points!");
        UpdateScoreDisplays();
        //if (FindObjectOfType<ScoreDisplay>() != null)
        //{
        //    FindObjectOfType<ScoreDisplay>().UpdateScore(score);
        //} else
        //{
        //    Debug.Log("Score: " + score);
        //}
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreDisplays();
        //if (FindObjectOfType<ScoreDisplay>() != null)
        //{
        //    FindObjectOfType<ScoreDisplay>().UpdateScore(score);
        //}
    }
    // Added by ZS to update the two variants of the score.
    private void UpdateScoreDisplays() 
    { 
        if (scoreDisplay != null) 
        { scoreDisplay.UpdateScore(score); 
        }
        if (deathScoreDisplay != null) 
        { 
            deathScoreDisplay.UpdateScore(score); 
        }
        if (winScoreDisplay != null)
        {
            winScoreDisplay.UpdateScore(score);
        }
        Debug.Log("Score:" + score); 
    }
}
