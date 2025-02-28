//ZS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    // audio parameter controller script
    [SerializeField] AudioParameterController parameterController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void ResumeGame()
    {
        CloseSettingsMenu(); // Added this so that the settings menu automatically closes in case it was open when the game is resumed. - NK
        parameterController.Unpaused();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.paused = false;
    }
    void PauseGame()
    {
        parameterController.Paused();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameManager.paused = true;

    }
    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }
    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        DataManager.Instance.SaveGame(); // Automatically saves the game when the player exits to main menu. - NK
        SceneManager.LoadScene("MainMenu"); 
    }
    public void QuitGame()
    {
        DataManager.Instance.SaveGame(); // Automatically saves the game when the player quits the game. - NK
        Debug.Log("Quitting game");
        Application.Quit();
    }
    public void PlayClickSound()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonSelect, this.transform.position);
    }
    public void PlayHoverSound()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonHover, this.transform.position);
    }
    
}