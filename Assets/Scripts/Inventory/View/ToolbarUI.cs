using Inventory.Controller;
using Items.Base;
using UnityEngine;

namespace Inventory.View
{
    public class ToolbarUI : InventoryUI
    {
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private GameObject slotSelector;

        public SlotUI SelectedSlotUI
        {
            get
            {
                if (slots == null || slots.Length == 0) return null;
                if (selectedSlot < 0 || selectedSlot >= slots.Length) return null;
                return slots[selectedSlot];
            }
        }

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
            SetItemEquipped();
        }

        public void SetItemEquipped()
        {
            ItemAmount itemAmount = inventory.GetItemByIndex(selectedSlot);
            ItemsInHand.Instance.SetItemEquipped(itemAmount.SoItem);
        }
    }
}