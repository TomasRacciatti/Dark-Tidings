using System;
using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour, IInventory
    {
        [SerializeField] private GameObject inventoryItemPrefab;
        [SerializeField] private InventorySlot[] toolbarSlots;
        [SerializeField] private InventorySlot[] inventorySlots;
        [SerializeField] private InventorySlot[] armorSlots;
        //[SerializeField] private InventorySlot[] bagpackSlots;
        [SerializeField] private InventorySlot selectedSlot;
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject backpackUI;
        [SerializeField] private GameObject slotSelector;
        [SerializeField] private GameObject bulletSelector;
        //public bool hasBagpack = false;

        private void Start()
        {
            ChangeSelectedSlot(toolbarSlots[0]);
        }

        public InventoryItem GetSelectedItem()
        {
            return selectedSlot.GetComponentInChildren<InventoryItem>();
        }
        
        public void ChangeSelectedSlot(int slot)
        {
            if (slot < toolbarSlots.Length)
            {
                ChangeSelectedSlot(toolbarSlots[slot]);
            }
        }

        public void ChangeSelectedSlot(InventorySlot slot)
        {
            selectedSlot = slot;
            slotSelector.transform.SetParent(selectedSlot.transform, false);
            slotSelector.transform.localPosition = Vector3.zero;
        }

        public bool ToggleInventory()
        {
            bool setActive = !inventoryUI.gameObject.activeSelf;
            inventoryUI.gameObject.SetActive(setActive);
            if (setActive)
            {
                // Mostrar y liberar el cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Ocultar y bloquear el cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            return setActive;
        }
        
        public int AddItem(ItemObject itemObject, int count)
        {
            InventoryItem itemSlot;

            //Find Stackeable Slot
            if (itemObject.stack > 1)
            {
                foreach (var slot in GetOrder(itemObject.type))
                {
                    itemSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (slot && itemSlot.itemObject == itemObject && itemSlot.count < itemObject.stack)
                    {
                        int remainingCount = Mathf.Clamp(count, 1, itemObject.stack - itemSlot.count);
                        itemSlot.AddCount(remainingCount);
                        count -= remainingCount;
                        if (count <= 0) return 0;
                    }
                }
            }

            //Find Empty Slot
            foreach (var slot in GetOrder(itemObject.type))
            {
                itemSlot = slot.GetComponentInChildren<InventoryItem>();
                if (!itemSlot)
                {
                    int remainingCount = Mathf.Clamp(count, 1, itemObject.stack);
                    SpawnItem(itemObject, slot, count);
                    count -= remainingCount;
                    if (count <= 0) return 0;
                }
            }

            //Full Inventory
            return count;
        }

        private void SpawnItem(ItemObject itemObject, InventorySlot slot, int count)
        {
            GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
            inventoryItem.SetItem(itemObject);
        }

        private IEnumerable<InventorySlot> GetOrder(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Weapon:
                    return GetSlotsInCustomOrder(toolbarSlots, inventorySlots );
                case ItemType.Tool:
                    return GetSlotsInCustomOrder(toolbarSlots, inventorySlots );
                case ItemType.Armor:
                    return GetSlotsInCustomOrder(inventorySlots, armorSlots, toolbarSlots );
                case ItemType.Material:
                    return GetSlotsInCustomOrder(inventorySlots );
                default:
                    return GetSlotsInCustomOrder(toolbarSlots, inventorySlots );
            }
        }

        private IEnumerable<InventorySlot> GetSlotsInCustomOrder(params InventorySlot[][] slotGroups)
        {
            foreach (var group in slotGroups)
            {
                foreach (var slot in group)
                {
                    yield return slot;
                }
            }
        }
        
        
        //interface
        public void AddItem(object item)
        {
            throw new NotImplementedException();
        }

        public bool RemoveItem(object item)
        {
            throw new NotImplementedException();
        }

        public List<object> GetItems()
        {
            throw new NotImplementedException();
        }

        public bool HasItem(object item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}