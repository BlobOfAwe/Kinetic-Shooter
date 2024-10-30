using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Sidebar : MonoBehaviour
{

    //This script handles the bulk of the pickup interaction
    public GameObject pickupSlot;
    public Transform sidebarPanel; 
    public GameObject tooltip;          
    public TMPro.TMP_Text tooltipText;
    public GameObject sidebarUI;
    private List<Pickup> collectedPickups = new List<Pickup>();
    void Start()
    {
        sidebarUI.SetActive(false);
        tooltip.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSidebar();
        }
        UpdateTooltipPosition();
    }
    public void ToggleSidebar()
    {
        sidebarUI.SetActive(!sidebarUI.activeSelf);
    }
    public void AddPickup(Pickup newPickup)
    {
        collectedPickups.Add(newPickup);
        GameObject newSlot = Instantiate(pickupSlot, sidebarPanel);
        Image icon = newSlot.GetComponent<Image>();
        icon.sprite = newPickup.icon;
        newSlot.AddComponent<ToolTipTrigger>().Initialize(this, newPickup.description);
    }
    public void ShowTooltip(string description)
    {
        tooltipText.text = description;
        tooltip.SetActive(true);
        PositionTooltip();
    }
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
    private void PositionTooltip()
    {
        Vector3 mousePosition = Input.mousePosition;
        tooltip.transform.position = mousePosition - new Vector3(30, 30, 0);
    }
    private void UpdateTooltipPosition()
    {
        if (tooltip.activeSelf)
        {
            Vector3 mousePosition = Input.mousePosition;
            RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
            float clampedX = Mathf.Clamp(mousePosition.x, tooltipRect.rect.width / 2, Screen.width - tooltipRect.rect.width / 2);
            float clampedY = Mathf.Clamp(mousePosition.y, tooltipRect.rect.height / 2, Screen.height - tooltipRect.rect.height / 2);
            tooltip.transform.position = new Vector3(clampedX, clampedY - 20, 0);
        }
    }

}



