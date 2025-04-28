using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.View
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public SlotType slotType;
        public int slotIndex;

        private InventoryItem _inventoryItem;

        public void SetSlotType(SlotType type, int index)
        {
            slotType = type;
            slotIndex = index;
        }

        public void SetItem(ItemAmount itemAmount)
        {
            if (itemAmount.Item == null || itemAmount.IsEmpty)
            {
                if (_inventoryItem != null)
                {
                    Destroy(_inventoryItem.gameObject);
                    _inventoryItem = null;
                }

                return;
            }

            if (_inventoryItem == null)
            {
                GameObject newItem = Instantiate(CanvasGameManager.Instance.inventoryManager.itemPrefab, transform);
                _inventoryItem = newItem.GetComponent<InventoryItem>();
            }

            _inventoryItem.SetItem(itemAmount.Item, itemAmount.Amount);
        }

        public void SetInventoryItem(InventoryItem inventoryItem)
        {
            _inventoryItem = inventoryItem;

            if (inventoryItem != null)
            {
                inventoryItem.parentTransform = transform;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            InventoryItem fromItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (fromItem == null) return;

            InventorySlot fromSlot = fromItem.parentTransform.GetComponent<InventorySlot>();
            InventoryItem toItem = _inventoryItem;
            
            fromSlot.SetInventoryItem(toItem);
            SetInventoryItem(fromItem);
        }
    }

    public enum SlotType
    {
        Inventory,
        Toolbar,
        Backpack,
        Armor,
        Crafting
    }
}