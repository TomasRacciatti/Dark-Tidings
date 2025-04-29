using System;
using Inventory.View;
using UnityEngine;

namespace Inventory.Controller
{
    public class Toolbar : InventoryView
    {
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private GameObject slotSelector;
        
        public InventorySlot SelectedSlot
        {
            get
            {
                if (slots == null || slots.Length == 0) return null;
                if (selectedSlot < 0 || selectedSlot >= slots.Length) return null;
                return slots[selectedSlot];
            }
        }

        protected override void Awake()
        {
            base.Awake();
            ChangeSelectedSlot(0);
        }
        
        public void ChangeSelectedSlot(int slot)
        {
            if (selectedSlot == slot) return;
            if (slot >= slots.Length) return;

            selectedSlot = slot;
            slotSelector.transform.SetParent(SelectedSlot.transform, false);
            slotSelector.transform.localPosition = Vector3.zero;
        }
        
        public InventoryItem GetSelectedItem()
        {
            return SelectedSlot.GetComponentInChildren<InventoryItem>();
        }

        public InventoryItem GetItem(InventoryItem item)
        {
            foreach (InventorySlot slot in slots)
            {
                InventoryItem existingItem = slot.GetComponentInChildren<InventoryItem>();
                if (existingItem != null && existingItem.originalItem == item)
                {
                    return existingItem;
                }
            }
            return null;
        }
    }
}