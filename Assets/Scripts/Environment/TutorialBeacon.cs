using Cinemachine;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class TutorialBeacon : Beacon
{
    [SerializeField]
    private int menuScene = 0;

    protected override IEnumerator WinGame()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(menuScene);
    }
}
