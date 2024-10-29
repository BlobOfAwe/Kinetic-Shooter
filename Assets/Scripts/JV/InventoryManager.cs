using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    
    public List<InventorySlot> inventory;
    [SerializeField] InventorySlot slotPrefab;
    [SerializeField] Transform slotParent;
    [SerializeField] int initialSize = 20;

    private void Awake()
    {
        try { slotPrefab.GetComponent<InventorySlot>(); } 
        catch { Debug.LogError("SlotPrefab does not contain component for Inventory slot"); return; }
        
        inventory = new List<InventorySlot>(initialSize);
        for (int i = 0; i < initialSize; i++)
        {
            inventory.Add(NewSlot());
        }
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
                slot.quantity = 1;
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
}
