using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Toolbar : MonoBehaviour
    {
        [SerializeField] private InventorySlot[] toolbarSlots;
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private GameObject slotSelector;

        public static Toolbar Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        private void Start()
        {
            ChangeSelectedSlot(0);
        }
        
        public void ChangeSelectedSlot(int slot)
        {
            if (selectedSlot == slot) return;
            if (slot >= toolbarSlots.Length) return;

            selectedSlot = slot;
            slotSelector.transform.SetParent(toolbarSlots[selectedSlot].transform, false);
            slotSelector.transform.localPosition = Vector3.zero;
        }
        
        public InventoryItem GetSelectedItem()
        {
            return toolbarSlots[selectedSlot].GetComponentInChildren<InventoryItem>();
        }

        public void EquipItem(ItemAmount itemAmount, int slot)
        {
            if (slot >= toolbarSlots.Length) return;
            
            InventoryItem inventoryItem = toolbarSlots[slot].GetComponentInChildren<InventoryItem>();
            if (!inventoryItem)
            {
                GameObject newItem = Instantiate(InventoryManager.Instance.itemPrefab, toolbarSlots[slot].transform);
                inventoryItem = newItem.GetComponent<InventoryItem>();
            }
            inventoryItem.SetItem(itemAmount.item, itemAmount.amount);
        }
    }
}