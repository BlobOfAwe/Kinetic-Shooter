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

    // audio parameter controller script
    [SerializeField] AudioParameterController parameterController;

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

        ScoreDisplay[] allLoadedDisplays = FindObjectsOfType<ScoreDisplay>(true);
        // Iterate through each loaded display and check the game object's name to assign it correctly
        if (allLoadedDisplays.Length == 3)
        {
            foreach (ScoreDisplay display in allLoadedDisplays)
            {
                switch (display.gameObject.name)
                {
                    case "ScoreDisplay":
                        scoreDisplay = display;
                        break;
                    case "DeathScoreDisplay":
                        deathScoreDisplay = display;
                        break;
                    case "WinScoreDisplay":
                        parameterController.EndingWin();
                        winScoreDisplay = display;
                        break;
                    default:
                        Debug.LogError(display.gameObject.name + " is not a valid Score Display Name");
                        break;
                }
            }
        }
        else { Debug.LogError("Incorrect number of score displays (" + allLoadedDisplays.Length + ") were detected in scene. Make sure there are exactly three score displays in the scene."); }

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
        //Debug.Log("+" + points + " points!");
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
        GameManager.score = score;
        if (scoreDisplay != null) 
        { 
            scoreDisplay.UpdateScoreText(); 
        }
        // Death and Win Displays are only updated after death or victory respectively
        //if (deathScoreDisplay != null) 
        //{ 
        //    deathScoreDisplay.UpdateScore(score); 
        //}
        //if (winScoreDisplay != null)
        //{
        //    winScoreDisplay.UpdateScore(score);
        //}
        //Debug.Log("Score:" + score); 
    }
}
