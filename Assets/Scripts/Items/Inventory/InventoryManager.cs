using System;
using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryItemPrefab;
        
        //Slots
        [SerializeField] private InventorySlot[] toolbarSlots;
        [SerializeField] private InventorySlot[] inventorySlots;
        [SerializeField] private InventorySlot[] armorSlots;
        [SerializeField] private InventorySlot[] backpackSlots;
        [SerializeField] private InventorySlot selectedSlot;
        
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject backpackUI;
        [SerializeField] private GameObject slotSelector;
        [SerializeField] private GameObject bulletSelector;
        [SerializeField] private GameObject backpack;
        [SerializeField] private GameObject backpackRoot;
        public bool hasBagpack = false;

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
        
        public bool ToggleBackpack()
        {
            hasBagpack = !hasBagpack;
            backpackUI.gameObject.SetActive(hasBagpack);
            return hasBagpack;
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

        //add item order
        private IEnumerable<InventorySlot> GetOrder(ItemType itemType)
        {
            List<InventorySlot> slotOrder = new();

            switch (itemType)
            {
                case ItemType.Weapon:
                case ItemType.Tool:
                    slotOrder.AddRange(toolbarSlots);
                    slotOrder.AddRange(inventorySlots);
                    break;

                case ItemType.Armor:
                    slotOrder.AddRange(armorSlots);
                    slotOrder.AddRange(inventorySlots);
                    slotOrder.AddRange(toolbarSlots);
                    break;

                case ItemType.Material:
                    slotOrder.AddRange(inventorySlots);
                    break;

                default:
                    slotOrder.AddRange(toolbarSlots);
                    slotOrder.AddRange(inventorySlots);
                    break;
            }

            if (hasBagpack)
            {
                slotOrder.AddRange(backpackSlots);
            }

            return GetSlotsInCustomOrder(slotOrder.ToArray());
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
    }
}