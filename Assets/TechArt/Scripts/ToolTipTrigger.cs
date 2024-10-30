using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //This script handles the intialization and management of the tooltips
    public Sidebar sidebar;
    private string description;
    private string title;

    public void Initialize(Sidebar sidebar, string description)
    {
        this.sidebar = sidebar;
        this.description = description;
    
    //this.description = title;
}

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        sidebar.ShowTooltip(description);
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