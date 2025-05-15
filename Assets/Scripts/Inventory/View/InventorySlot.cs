using UnityEngine;
using UnityEngine.EventSystems;
using Characters.Player;
using Inventory.Controller;
using Items.Base;
using UnityEngine.Events;

namespace Inventory.View
{
    public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        public SlotType slotType;
        public int slotIndex;
        public UnityEvent onClickAction;

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
                GameObject newItem = Instantiate(CanvasManager.Instance.inventoryManager.itemSlotPrefab, transform);
                item = newItem.GetComponent<InventoryItem>();
            }

            item.SetItem(itemAmount);
        }

        public ItemAmount GetItemAmount()
        {
            InventoryItem item = GetComponentInChildren<InventoryItem>();
            if (item == null)
            {
                return new ItemAmount();
            }

            return item.itemAmount;
        }

        public SO_Item GetItemObject()
        {
            InventoryItem item = GetComponentInChildren<InventoryItem>();
            if (item == null)
            {
                return null;
            }

            return GetItemAmount().GetSoItem;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClickAction?.Invoke();
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
            if (fromSlot.slotType != CanvasManager.Instance.inventoryManager.inventorySlotType ||
                slotType != CanvasManager.Instance.inventoryManager.inventorySlotType)
                return false;

            fromItem.SetParent(transform);
            if (toItem)
            {
                toItem.SetParent(fromSlot.transform);
            }

            PlayerController.Instance.inventory.SwapItems(fromSlot.slotIndex, slotIndex);
            return true;
        }

        private bool HandleInventoryToToolbar(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem) //todo sacar el to item y hacerlo por toolbar esto
        {
            if (fromSlot.slotType != CanvasManager.Instance.inventoryManager.inventorySlotType ||
                slotType != CanvasManager.Instance.inventoryManager.toolbarSlotType)
                return false;

            if (!fromItem.itemAmount.ItemInstance.IsEquippable) return true;

            Toolbar toolbar = PlayerCharacter.Instance.GetComponent<Toolbar>();
            toolbar.SetIndex(slotIndex, fromSlot.slotIndex);


            if (toItem != null && fromItem == toItem.originalItem) return true;

            if (toItem != null) toItem.originalItem.SetEquipable(-1);

            InventoryItem existingItem = CanvasManager.Instance.inventoryManager.toolbarUI.GetItem(fromItem);
            if (existingItem != null) Destroy(existingItem.gameObject);

            if (toItem == null)
            {
                GameObject newItem = Instantiate(CanvasManager.Instance.inventoryManager.itemSlotPrefab, transform);
                toItem = newItem.GetComponent<InventoryItem>();
            }

            toItem.SetItem(fromItem.itemAmount, fromItem);
            fromItem.SetEquipable(slotIndex);
            toItem.SetEquipable(slotIndex);
            CanvasManager.Instance.inventoryManager.toolbarUI.SetItemEquipped();
            return true;
        }

        private bool HandleToolbarToToolbar(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasManager.Instance.inventoryManager.toolbarSlotType ||
                slotType != CanvasManager.Instance.inventoryManager.toolbarSlotType)
                return false;

            fromItem.SetParent(transform);
            fromItem.SetEquipable(slotIndex);
            fromItem.originalItem.SetEquipable(slotIndex);
            if (toItem)
            {
                toItem.SetParent(fromSlot.transform);
                toItem.SetEquipable(fromSlot.slotIndex);
                toItem.originalItem.SetEquipable(fromSlot.slotIndex);
            }
            Toolbar toolbar = PlayerCharacter.Instance.GetComponent<Toolbar>();
            toolbar.SwapIndexes(slotIndex, fromSlot.slotIndex);

            CanvasManager.Instance.inventoryManager.toolbarUI.SetItemEquipped();
            return true;
        }

        private bool HandleToolbarToInventory(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasManager.Instance.inventoryManager.toolbarSlotType ||
                slotType != CanvasManager.Instance.inventoryManager.inventorySlotType)
                return false;
            
            Toolbar toolbar = PlayerCharacter.Instance.GetComponent<Toolbar>();
            toolbar.SetIndex(toolbar.GetIndex(fromSlot.slotIndex), -1);

            fromItem.originalItem.SetEquipable(-1);
            Destroy(fromItem.gameObject);
            if (CanvasManager.Instance.inventoryManager.toolbarUI.SelectedSlot.slotIndex == fromSlot.slotIndex)
            {
                ItemsInHand.Instance.SetItemEquipped(null);
            }

            return true;
        }
    }
}