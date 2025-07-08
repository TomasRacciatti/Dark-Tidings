using Effects;
using Inventory.Controller;
using Items.Base;
using UnityEngine;

namespace Inventory.View
{
    public class ToolbarUI : InventoryUI
    {
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private GameObject slotSelector;
        [SerializeField] private CanvasGroup toolbarPanel;

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

        public void Show()
        {
            if (toolbarPanel.alpha >= 0.99f) return;
            StartCoroutine(Effect.Fade(toolbarPanel ,0, 1, 0.4f));
        }
        
        public void Hide()
        {
            if (toolbarPanel.alpha <= 0.01f) return;
            StartCoroutine(Effect.Fade(toolbarPanel ,1, 0, 0.4f));
        }
    }
}