using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioTestScene : MonoBehaviour
{

    [SerializeField] AudioParameterController parameterController;

    private void Start()
    {
        //parameterController = GameObject.FindGameObjectWithTag("AudioParameterController").GetComponent<AudioParameterController>();
    }

    public void IndustrialSelection()
    {
        AudioManager.songSelection = 1;
        SceneManager.LoadScene("AudioAlphaScene");
    }
    public void JungleSelection()
    {
        AudioManager.songSelection = 2;
        SceneManager.LoadScene("AudioAlphaScene");
    }
    public void LavaSelection()
    {
        AudioManager.songSelection = 3;
        SceneManager.LoadScene("AudioAlphaScene");
    }

}
