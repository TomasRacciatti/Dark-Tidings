using System;
using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryItemPrefab;
        [SerializeField] private InventorySlot[] toolbarSlots;
        [SerializeField] private InventorySlot[] inventorySlots;
        [SerializeField] private InventorySlot[] armorSlots;
        //[SerializeField] private InventorySlot[] bagpackSlots;
        [SerializeField] private InventorySlot selectedSlot;
        [SerializeField] private GameObject selector;
        //public bool hasBagpack = false;

        private void Start()
        {
            ChangeSelectedSlot(toolbarSlots[0]);
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
            selector.transform.SetParent(selectedSlot.transform, false);
            selector.transform.localPosition = Vector3.zero;
        }
        
        public int AddItem(Item item, int count)
        {
            InventoryItem itemSlot;

            //Find Stackeable Slot
            if (item.stack > 1)
            {
                foreach (var slot in GetOrder(item.type))
                {
                    itemSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (slot && itemSlot.item == item && itemSlot.count < item.stack)
                    {
                        int remainingCount = Mathf.Clamp(count, 1, item.stack - itemSlot.count);
                        itemSlot.AddCount(remainingCount);
                        count -= remainingCount;
                        if (count <= 0) return 0;
                    }
                }
            }

            //Find Empty Slot
            foreach (var slot in GetOrder(item.type))
            {
                itemSlot = slot.GetComponentInChildren<InventoryItem>();
                if (!itemSlot)
                {
                    int remainingCount = Mathf.Clamp(count, 1, item.stack);
                    SpawnItem(item, slot, count);
                    count -= remainingCount;
                    if (count <= 0) return 0;
                }
            }

            //Full Inventory
            return count;
        }

        private void SpawnItem(Item item, InventorySlot slot, int count)
        {
            GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
            inventoryItem.SetItem(item);
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
                    return GetSlotsInCustomOrder(inventorySlots, toolbarSlots );
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
    }
}