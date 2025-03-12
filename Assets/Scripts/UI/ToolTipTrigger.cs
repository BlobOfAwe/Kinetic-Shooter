// ## - ZS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //This script handles the intialization and management of the tooltips
    public InventoryManager sidebar;
    private string description;
    public string title;
    public List<StatGrabber> modifications;
    public void Initialize(InventoryManager sidebar, string description)
    {
        this.sidebar = sidebar;
        this.description = description;
    
    //this.description = title;
}

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        sidebar.ShowTooltip(title, description, modifications);
        //sidebar.ShowTooltip(title);
    }
    //Coroutine needed to avoid the tooltip flickering non stop because of the raycasting interaction.
    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(HideTooltip());
    }
    private IEnumerator HideTooltip()
    {
      
        yield return new WaitForSeconds(0.1f);
        sidebar.HideTooltip();
    }
}