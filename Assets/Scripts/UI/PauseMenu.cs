//ZS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    // audio parameter controller script
    [SerializeField] AudioParameterController parameterController;

    // Added by Nathaniel Klassen
    private EventSystem eventSystem;
    [SerializeField]
    private GameObject defaultButton;
    [SerializeField]
    private GameObject settingsButton;
    [SerializeField]
    private GameObject defaultAudioSlider;

    // This is now handled in PlayerBehaviour.OnPauseGame() - NK
    /*void Update()
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
    }*/

    // All EventSystem related code added by Nathaniel Klassen
    private void OnEnable()
    {
        eventSystem = EventSystem.current;
    }

    public void ResumeGame()
    {
        CloseSettingsMenu(); // Added this so that the settings menu automatically closes in case it was open when the game is resumed. - NK
        parameterController.Unpaused();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.paused = false;
    }
    public void PauseGame()
    {
        parameterController.Paused();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameManager.paused = true;
        eventSystem.SetSelectedGameObject(defaultButton);
    }
    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defaultAudioSlider);
    }
    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(settingsButton);
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.paused = false; // Added this so that the game no longer behaves as though the game is paused when it isn't. - NK
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

    // Added by Nathaniel Klassen
    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (context.canceled && eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(defaultButton);
        }
    }
}