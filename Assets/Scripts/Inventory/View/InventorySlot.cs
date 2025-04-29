using UnityEngine;
using UnityEngine.EventSystems;
using Characters.Player;

namespace Inventory.View
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public SlotType slotType;
        public int slotIndex;

        public void SetSlotType(SlotType type, int index)
        {
            slotType = type;
            slotIndex = index;
        }

        public void SetItem(ItemAmount itemAmount)
        {
            InventoryItem item = GetComponentInChildren<InventoryItem>();
            if (itemAmount.IsEmpty)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
                return;
            }

            if (item == null)
            {
                GameObject newItem = Instantiate(CanvasGameManager.Instance.inventoryManager.itemPrefab, transform);
                item = newItem.GetComponent<InventoryItem>();
            }

            item.SetItem(itemAmount);
        }

        public void OnDrop(PointerEventData eventData)
        {
            InventoryItem fromItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (fromItem == null)
            {
                transform.localPosition = Vector3.zero;
                return;
            }

            InventorySlot fromSlot = fromItem.GetComponentInParent<InventorySlot>();
            InventoryItem toItem = GetComponentInChildren<InventoryItem>();

            if (HandleInventoryToInventory(fromSlot, fromItem, toItem)) return;
            if (HandleInventoryToToolbar(fromSlot, fromItem, toItem)) return;
            if (HandleToolbarToToolbar(fromSlot, fromItem, toItem)) return;
            if (HandleToolbarToInventory(fromSlot, fromItem, toItem)) return;
        }

        private bool HandleInventoryToInventory(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType ||
                slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType)
                return false;

            fromItem.SetParent(transform);
            if (toItem)
            {
                toItem.SetParent(fromSlot.transform);
            }
            PlayerController.Instance.inventory.SwapItems(fromSlot.slotIndex, slotIndex);
            return true;
        }

        private bool HandleInventoryToToolbar(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType ||
                slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType)
                return false;

            if (!fromItem.itemAmount.ItemInstance.IsEquippable) return true;
            
            if (toItem != null && fromItem == toItem.originalItem) return true;
            
            if (toItem != null) toItem.originalItem.SetEquipable(-1);

            InventoryItem existingItem = CanvasGameManager.Instance.inventoryManager.toolbar.GetItem(fromItem);
            if (existingItem != null) Destroy(existingItem.gameObject);

            if (toItem == null)
            {
                GameObject newItem = Instantiate(CanvasGameManager.Instance.inventoryManager.itemPrefab, transform);
                toItem = newItem.GetComponent<InventoryItem>();
            }

            toItem.SetItem(fromItem.itemAmount, fromItem);
            fromItem.SetEquipable(slotIndex);
            toItem.SetEquipable(slotIndex);
            return true;
        }

        private bool HandleToolbarToToolbar(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType ||
                slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType)
                return false;
            
            fromItem.SetParent(transform);
            fromItem.SetEquipable(slotIndex);
            fromItem.originalItem.SetEquipable(slotIndex);
            if (toItem)
            {
                toItem.SetParent(fromSlot.transform);
                toItem.SetEquipable(fromSlot.slotIndex);
                toItem.originalItem.SetEquipable(slotIndex);
            }
            return true;
        }
        
        private bool HandleToolbarToInventory(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType ||
                slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType)
                return false;
            
            fromItem.originalItem.SetEquipable(-1);
            Destroy(fromItem.gameObject);
            return true;
        }
    }
}