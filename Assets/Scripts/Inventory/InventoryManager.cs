// ## - JV
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    
    public List<InventorySlot> inventory;
    [SerializeField] InventorySlot slotPrefab;
    [SerializeField] Transform slotParent;
    [SerializeField] int initialSize = 20;

    [SerializeField] GameObject tooltip;
    [SerializeField] private TMPro.TMP_Text tooltipText;
    [SerializeField] private TMPro.TMP_Text tooltipTitle;
    [SerializeField] private Transform statIconsPanel;  
    [SerializeField] private GameObject statIconPrefab;
    [SerializeField] private Sprite upArrowSprite;     
    [SerializeField] private Sprite downArrowSprite;

    private void Awake()
    {
        // If there is another inventory manager in the scene, get a reference to it
        InventoryManager[] allInvs = FindObjectsByType<InventoryManager>(FindObjectsSortMode.None);
        InventoryManager otherInv = null;
        if (allInvs.Length > 1) 
        { 
            foreach (InventoryManager inv in allInvs)
            {
                if (inv != this)
                {
                    otherInv = inv;
                }
            }
        }
        
        try { slotPrefab.GetComponent<InventorySlot>(); } 
        catch { Debug.LogError("SlotPrefab does not contain component for Inventory slot"); return; }
        
        // Initialize the new inventory
        inventory = new List<InventorySlot>(initialSize);

        // If there is a second inventory manager in the scene
        if (otherInv != null)
        {
            // For each slot in the new inventory
            for (int i = 0; i < initialSize; i++)
            {
                // If an item populates that slot in the old inventory, move it to the new inventory
                if (otherInv.inventory[i].item != null)
                {
                    inventory.Add(otherInv.inventory[i]);
                    otherInv.inventory[i].transform.parent = slotParent;

                    // Moves the item back to the active scene, removing the DontDestroyOnLoad tag
                    SceneManager.MoveGameObjectToScene(otherInv.inventory[i].item.gameObject, SceneManager.GetActiveScene());
                }
                // Otherwise, generate a new slot
                else
                {
                    inventory.Add(NewSlot());
                }
            }
            otherInv.gameObject.SetActive(false);
            Destroy(otherInv.gameObject);
        }

        // If this is the only inventory manager, generate a fully new inventory
        else
        {
            for (int i = 0; i < initialSize; i++)
            {
                inventory.Add(NewSlot());
            }
        }
    }

    private void Start()
    {
        slotParent.gameObject.SetActive(false);
        tooltip.SetActive(false);
        GetComponentInParent<Entity>().UpdateStats();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSidebar();
        }
        UpdateTooltipPosition();
    }


    public void AddItem(Item item)
    {
        // For each item in the inventory...
        foreach (InventorySlot slot in inventory)
        {
            // If the slot is populated
            if (slot.gameObject.activeSelf)
            {
                // If the player already has an item of the same type, add one to the quantity, then return
                if (slot.item.GetType() == item.GetType())
                {
                    slot.quantity++;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = "x" + slot.quantity.ToString();
                    return;
                }
            }

            // If we reach an unpopulated slot without finding a match, populate the slot with the item
            else 
            {
                slot.gameObject.SetActive(true);
                slot.item = item;
                var tooltipTrigger = slot.gameObject.GetComponent<ToolTipTrigger>();
                tooltipTrigger.Initialize(this, item.description);
                tooltipTrigger.title = item.title;
                tooltipTrigger.modifications = item.statModifications;
                //slot.gameObject.GetComponent<ToolTipTrigger>().Initialize(this, item.description);
                slot.quantity = 1;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = "x" + slot.quantity.ToString();
                slot.sprite = item.sprite;
                

                slot.gameObject.GetComponent<Image>().sprite = slot.sprite;
                return;
            }
        }

        // If the player does not have the item in their inventory, and the existing inventory space is exhausted
        // create a new inventory slot and add the item
        InventorySlot temp = NewSlot();
        temp.item = item;
        temp.quantity = 1;
        temp.sprite = item.sprite;
        temp.gameObject.GetComponent<Image>().sprite = temp.sprite;
        temp.GetComponentInChildren<TextMeshProUGUI>().text = "x" + temp.quantity.ToString();
        temp.gameObject.SetActive(true);
        inventory.Add(temp);
        Debug.LogWarning("Initial inventory size exceeded. Instantiated overflow inventory slot at runtime.");
    }

    private InventorySlot NewSlot()
    {
        var obj = Instantiate(slotPrefab, slotParent);
        obj.gameObject.SetActive(false);
        return obj.GetComponent<InventorySlot>();
    }

    public void PreserveInventory()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        foreach (InventorySlot slot in inventory)
        {
            if (slot.item != null)
            {
                DontDestroyOnLoad(slot.item);
            }
        }
    }

    // ------------------------
    // All of the following code is taken from the now obsolete Sidebar.cs
    // ------------------------
    public void ToggleSidebar()
    {
        slotParent.gameObject.SetActive(!slotParent.gameObject.activeSelf);
    }

    public void ShowTooltip(string title, string description, List<StatGrabber> modifications)
    {
        tooltipTitle.text = title;
        tooltipText.text = description;
        foreach (Transform child in statIconsPanel)
        {
            Destroy(child.gameObject);
        }
        int count = Mathf.Min(modifications.Count, 6);
        for (int i = 0; i < count; i++)
        {
            var mod = modifications[i];
            GameObject iconObj = Instantiate(statIconPrefab, statIconsPanel);
            Image iconImage = iconObj.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = mod.statIcon;
            }
            Transform arrowTransform = iconObj.transform.Find("Arrow");
            if (arrowTransform != null)
            {
                Image arrowImage = arrowTransform.GetComponent<Image>();
                if (arrowImage != null)
                {
                    arrowImage.sprite = mod.isPositive ? upArrowSprite : downArrowSprite;
                }
            }
        }
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
