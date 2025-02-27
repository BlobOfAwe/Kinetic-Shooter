using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;

    public void StartAsyncLoad()
    {
        // If you are loading from the main menu scene, the run is starting, set the difficulty to 0
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameManager.difficultyCoefficient = 0;
            GameManager.currentLevel = 0;
        }
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }
    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
