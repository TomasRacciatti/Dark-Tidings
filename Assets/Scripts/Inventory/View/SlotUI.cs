using UnityEngine;
using UnityEngine.EventSystems;
using Characters.Player;
using Inventory.Controller;
using Items.Base;
using Managers;
using UnityEngine.Events;

namespace Inventory.View
{
    public class SlotUI : MonoBehaviour, IDropHandler
    {
        public InventoryUI InventoryUI { get; private set; }
        public int SlotIndex { get; private set; }

        public void Initialize(InventoryUI inventoryUI, int slotIndex)
        {
            InventoryUI = inventoryUI;
            SlotIndex = slotIndex;
        }

        public void SetItem(ItemAmount itemAmount)
        {
            ItemUI itemUI = GetComponentInChildren<ItemUI>();
            if (itemAmount.IsEmpty)
            {
                if (itemUI != null)
                {
                    Destroy(itemUI.gameObject);
                }

                return;
            }

            if (itemUI == null)
            {
                GameObject newItem = Instantiate(GameManager.Canvas.inventoryManager.itemSlotPrefab, transform);
                itemUI = newItem.GetComponent<ItemUI>();
            }

            itemUI.SetItem(itemAmount);
        }

        public ItemAmount GetItemAmount()
        {
            ItemUI itemUI = GetComponentInChildren<ItemUI>();
            if (itemUI == null)
            {
                return new ItemAmount();
            }

            return itemUI.itemAmount;
        }

        public SO_Item GetItemObject()
        {
            ItemUI itemUI = GetComponentInChildren<ItemUI>();
            if (itemUI == null)
            {
                return null;
            }

            return GetItemAmount().SoItem;
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemUI fromItemUI = eventData.pointerDrag.GetComponent<ItemUI>();
            SlotUI fromSlotUI = fromItemUI.GetComponentInParent<SlotUI>();

            if (fromSlotUI == null) return;

            fromSlotUI.InventoryUI.Inventory.TransferIndexToIndex(InventoryUI.Inventory, fromSlotUI.SlotIndex,
                SlotIndex);
        }
    }
}