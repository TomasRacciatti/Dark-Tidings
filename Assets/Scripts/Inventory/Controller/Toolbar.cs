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

        protected override void Awake() //no se ejecuta nose porque
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
    }
}