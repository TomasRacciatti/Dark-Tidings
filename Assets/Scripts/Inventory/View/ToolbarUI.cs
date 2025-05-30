using Inventory.Controller;
using Items.Base;
using UnityEngine;

namespace Inventory.View
{
    public class ToolbarUI : InventoryUI
    {
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private GameObject slotSelector;

        private SlotUI SelectedSlotUI => slots[selectedSlot];

        protected override void Start()
        {
            base.Start();
            ChangeSelectedSlot(0);
        }

        public void ChangeSelectedSlot(int slot)
        {
            if (selectedSlot == slot) return;
            if (slot >= slots.Length) return;

            selectedSlot = slot;
            slotSelector.transform.SetParent(SelectedSlotUI.transform, false);
            slotSelector.transform.localPosition = Vector3.zero;
        }
    }
}