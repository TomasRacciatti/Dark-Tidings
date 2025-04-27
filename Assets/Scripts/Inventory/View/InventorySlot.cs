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
            if (itemAmount.IsEmpty && _inventoryItem != null)
            {
                Destroy(_inventoryItem.gameObject);
                _inventoryItem = null;
                return;
            }

            if (_inventoryItem == null)
            {
                GameObject newItem = Instantiate(CanvasGameManager.Instance.inventoryManager.itemPrefab, transform);
                _inventoryItem = newItem.GetComponent<InventoryItem>();
            }

            _inventoryItem.SetItem(itemAmount);
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

            // Si ambos slots son del mismo tipo, solo intercambiamos los items
            if (slotType == fromSlot.slotType)
            {
                fromSlot.SetInventoryItem(toItem);
                SetInventoryItem(fromItem);
                return;
            }

            // INVENTARIO -> TOOLBAR
            if (slotType == CanvasGameManager.Instance.inventoryManager.toolbarSlotType &&
                fromSlot.slotType == CanvasGameManager.Instance.inventoryManager.inventorySlotType)
            {
                if (!fromItem.itemAmount.Item.IsEquippable) return;

                if (toItem != null && toItem.originalItem != null)
                {
                    toItem.originalItem.SetEquipable(-1);
                }

                if (toItem == null)
                {
                    GameObject newItem = Instantiate(CanvasGameManager.Instance.inventoryManager.itemPrefab, transform);
                    _inventoryItem = newItem.GetComponent<InventoryItem>();
                }

                _inventoryItem.SetItem(fromItem.itemAmount, fromItem);
                _inventoryItem.SetEquipable(slotIndex);
                _inventoryItem.originalItem.SetEquipable(slotIndex);

                fromSlot.SetInventoryItem(null);
            }
            // TOOLBAR -> INVENTARIO
            else if (slotType == CanvasGameManager.Instance.inventoryManager.inventorySlotType &&
                     fromSlot.slotType == CanvasGameManager.Instance.inventoryManager.toolbarSlotType)
            {
                fromItem.SetEquipable(-1);
                if (fromItem.originalItem != null)
                {
                    fromItem.originalItem.SetEquipable(-1);
                }

                fromSlot.SetInventoryItem(null);
                fromSlot.SetInventoryItem(toItem);
                SetInventoryItem(fromItem);
            }
        }
    }
}