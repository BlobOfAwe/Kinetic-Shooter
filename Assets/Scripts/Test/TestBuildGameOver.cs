// ## - NK
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBuildGameOver : MonoBehaviour
{
    [SerializeField]
    private int restartScene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(restartScene);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.Backspace))
            SceneManager.LoadScene(0);
    }
}
