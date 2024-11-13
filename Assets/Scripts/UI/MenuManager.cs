using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Animator titleAnimator;
    private bool isExpanded = false;


    public void ToggleMenu()
    {
        isExpanded = !isExpanded;
        titleAnimator.SetBool("isExpanded", isExpanded);
    }
}