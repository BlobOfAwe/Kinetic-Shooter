//ZS
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Animator animator;

    // Added by Nathaniel Klassen
    private EventSystem eventSystem;
    [SerializeField]
    private GameObject defaultButtonMain;
    [SerializeField]
    private GameObject defaultButtonLoadout;
    [SerializeField]
    private GameObject defaultButtonLogBook;
    [SerializeField]
    private GameObject defaultButtonSettings;
    [SerializeField]
    private GameObject defaultButtonAudio;

    // All EventSystem related code added by Nathaniel Klassen
    private void OnEnable()
    {
        eventSystem = EventSystem.current;
    }

    //use AsynchLoad script instead
    public void StartGame()
    {
        SceneManager.LoadScene("LoadoutSelectionScene"); //change to whichever scene we are loading
    }
    public void ToggleMainMenu()
    {
        bool isMainMenu = animator.GetBool("isMainMenu");
        animator.SetBool("isMainMenu", !isMainMenu);
        eventSystem.SetSelectedGameObject(defaultButtonMain);
    }
    public void ToggleSettings()
    {
        bool isSettings = animator.GetBool("isSettings");
        animator.SetBool("isSettings", !isSettings);
        if (isSettings)
        {
            eventSystem.SetSelectedGameObject(defaultButtonMain);
        } else
        {
            eventSystem.SetSelectedGameObject(defaultButtonSettings);
        }
    }
    public void ToggleAudioPanel()
    {
        bool isAudioPanel = animator.GetBool("isAudioPanel");
        animator.SetBool("isAudioPanel", !isAudioPanel);
        if (isAudioPanel)
        {
            eventSystem.SetSelectedGameObject(defaultButtonSettings);
        } else
        {
            eventSystem.SetSelectedGameObject(defaultButtonAudio);
        }
    }
    public void ToggleLoadoutMenu()
    {
        bool isLoadoutMenu = animator.GetBool("isLoadoutMenu");
        animator.SetBool("isLoadoutMenu", !isLoadoutMenu);
        if (isLoadoutMenu)
        {
            eventSystem.SetSelectedGameObject(defaultButtonMain);
        } else
        {
            eventSystem.SetSelectedGameObject(defaultButtonLoadout);
        }
    }
    public void ToggleLogBookMenu()
    {
        bool isLoadoutMenu = animator.GetBool("isLogBookMenu");
        animator.SetBool("isLogBookMenu", !isLoadoutMenu);
        if (isLoadoutMenu)
        {
            eventSystem.SetSelectedGameObject(defaultButtonMain);
        } else
        {
            eventSystem.SetSelectedGameObject(defaultButtonLogBook);
        }
    }
    public void ReturnToMainMenu()
    {
        animator.SetBool("isTitleScreen", false);
        animator.SetBool("isLoadoutMenu", false);
        animator.SetBool("isMainMenu", true);
        animator.SetBool("isSettings", false);
        animator.SetBool("isAudioPanel", false);
        animator.SetBool("isLogBookMenu", false);
        eventSystem.SetSelectedGameObject(defaultButtonMain);
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
            if (animator.GetBool("isLoadoutMenu"))
            {
                eventSystem.SetSelectedGameObject(defaultButtonLoadout);
            } else if (animator.GetBool("isLogBookMenu"))
            {
                eventSystem.SetSelectedGameObject(defaultButtonLogBook);
            } else if (animator.GetBool("isSettings"))
            {
                eventSystem.SetSelectedGameObject(defaultButtonSettings);
            } else
            {
                eventSystem.SetSelectedGameObject(defaultButtonMain);
            }
        }
    }
}