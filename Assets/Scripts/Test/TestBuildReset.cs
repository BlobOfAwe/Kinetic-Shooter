// ## - NK
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBuildReset : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.Backspace))
            SceneManager.LoadScene(0);
    }
}