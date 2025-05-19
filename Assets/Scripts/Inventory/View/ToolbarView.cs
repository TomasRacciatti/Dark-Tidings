using Inventory.Controller;
using UnityEngine;

namespace Inventory.View
{
    public class ToolbarView : InventoryView
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
            SetItemEquipped();
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

        public void SetItemEquipped()
        {
            InventoryItem item = SelectedSlot.GetComponentInChildren<InventoryItem>();
            ItemsInHand.Instance.SetItemEquipped(item == null ? null : item.itemAmount.ItemInstance.SoItem);
        }
    }
}