// ## - JV
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    
    public List<InventorySlot> inventory;
    [SerializeField] InventorySlot slotPrefab;
    [SerializeField] Transform slotParent;
    [SerializeField] GameObject inventoryBackground;
    [SerializeField] int initialSize = 20;

    [SerializeField] GameObject tooltip;
    [SerializeField] private TMPro.TMP_Text tooltipText;
    [SerializeField] private TMPro.TMP_Text tooltipTitle;
    [SerializeField] private Transform statIconsPanel;  
    [SerializeField] private GameObject statIconPrefab;
    [SerializeField] private Sprite upArrowSprite;     
    [SerializeField] private Sprite downArrowSprite;
    [SerializeField] private TMP_Text pickupText;
    [SerializeField] private CanvasGroup pickupCanvasGroup;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float fadeDuration = 0.5f;
    public bool isPaused; // Made public - NK
    private Coroutine currentNotificationRoutine;
    [Header("Tooltip Positioning")]
    [SerializeField] private float verticalOffset = 20f;
    [SerializeField] private float edgePadding = 10f;


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
        // This is now done in PlayerBehaviour.Inventory() - NK
        /*if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSidebar();
        }*/
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
                ShowPickupNotification(item.description);
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
        bool isActive = slotParent.gameObject.activeSelf;
        bool isBackgroundActive = inventoryBackground.activeSelf;
        inventoryBackground.SetActive(!isBackgroundActive);
        slotParent.gameObject.SetActive(!isActive);
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        GameManager.paused = isPaused;
        if (!slotParent.gameObject.activeSelf)
        {
            HideTooltip();
        }
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
        RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
        tooltipRect.pivot = new Vector2(0, 1);
        Vector3 desiredPosition = Input.mousePosition + new Vector3(0, -verticalOffset, 0);
        tooltipRect.position = GetClampedTooltipPosition(desiredPosition, tooltipRect);
    }
    private void UpdateTooltipPosition()
    {
        if (tooltip.activeSelf)
        {
            RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
            Vector3 desiredPosition = Input.mousePosition + new Vector3(0, -verticalOffset, 0);
            tooltipRect.position = GetClampedTooltipPosition(desiredPosition, tooltipRect);
        }
        if (isPaused && slotParent.gameObject.activeSelf && tooltip.activeSelf)
        {
            if (!IsMouseOverInventorySlot())
            {
                HideTooltip();
            }
        }
    }

    private Vector3 GetClampedTooltipPosition(Vector3 desiredPosition, RectTransform tooltipRect)
    {
        float tooltipWidth = tooltipRect.rect.width;
        float tooltipHeight = tooltipRect.rect.height;

        float minX = edgePadding;
        float maxX = Screen.width - tooltipWidth - edgePadding;
        float minY = tooltipHeight + edgePadding;
        float maxY = Screen.height - edgePadding;

        return new Vector3(
            Mathf.Clamp(desiredPosition.x, minX, maxX),
            Mathf.Clamp(desiredPosition.y, minY, maxY),
            0
        );
    }
    //Following is for the upgrade notification display Z.S
    public void ShowPickupNotification(string description)
    {
        if (currentNotificationRoutine != null)
        {
            StopCoroutine(currentNotificationRoutine);
        }
        pickupText.text = $"{description}";
        currentNotificationRoutine = StartCoroutine(ShowNotificationRoutine());
    }

    private IEnumerator ShowNotificationRoutine()
    {
        pickupCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayDuration);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            pickupCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pickupCanvasGroup.alpha = 0f;
        currentNotificationRoutine = null;
    }
    private bool IsMouseOverInventorySlot()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<InventorySlot>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
