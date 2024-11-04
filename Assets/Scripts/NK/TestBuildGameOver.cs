// ## - NK
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBuildGameOver : MonoBehaviour
{
    [SerializeField]
    private int restartScene;

    public void OnRestart()
    {
        SceneManager.LoadScene(restartScene);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
