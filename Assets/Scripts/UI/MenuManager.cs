//ZS
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Animator animator;
    //use AsynchLoad script instead
    public void StartGame()
    {
        SceneManager.LoadScene("LoadoutSelectionScene"); //change to whichever scene we are loading
    }
    public void ToggleMainMenu()
    {
        bool isMainMenu = animator.GetBool("isMainMenu");
        animator.SetBool("isMainMenu", !isMainMenu);
    }
    public void ToggleSettings()
    {
        bool isSettings = animator.GetBool("isSettings");
        animator.SetBool("isSettings", !isSettings);
    }
    public void ToggleAudioPanel()
    {
        bool isAudioPanel = animator.GetBool("isAudioPanel");
        animator.SetBool("isAudioPanel", !isAudioPanel);
    }
    public void ToggleLoadoutMenu()
    {
        bool isLoadoutMenu = animator.GetBool("isLoadoutMenu");
        animator.SetBool("isLoadoutMenu", !isLoadoutMenu);
    }
    public void ToggleLogBookMenu()
    {
        bool isLoadoutMenu = animator.GetBool("isLogBookMenu");
        animator.SetBool("isLogBookMenu", !isLoadoutMenu);
    }
    public void ReturnToMainMenu()
    {
        animator.SetBool("isTitleScreen", false);
        animator.SetBool("isLoadoutMenu", false);
        animator.SetBool("isMainMenu", true);
        animator.SetBool("isSettings", false);
        animator.SetBool("isAudioPanel", false);
        animator.SetBool("isLogBookMenu", false);
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