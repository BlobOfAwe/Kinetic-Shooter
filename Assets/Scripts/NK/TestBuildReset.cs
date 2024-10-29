using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBuildReset : MonoBehaviour
{
    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
