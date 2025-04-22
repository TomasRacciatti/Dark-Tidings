using System.Collections;
using System.Collections.Generic;
using Inventory;
using TMPro;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventorySlot[] slots;

    public void SetItem(int index, ItemAmount itemAmount)
    {
        if (index < 0 || index >= slots.Length) return; //devuelve si no tiene slots

        InventoryItem inventoryItem = slots[index].GetComponentInChildren<InventoryItem>();
        if (!inventoryItem)
        {
            GameObject newItem = Instantiate(InventoryManager.Instance.itemPrefab, slots[index].transform);
            inventoryItem = newItem.GetComponent<InventoryItem>();
        }
        inventoryItem.SetItem(itemAmount.item, itemAmount.amount);
    }
}
