using UnityEngine;
using UnityEngine.EventSystems;
using Characters.Player;
using Inventory.Controller;
using Items.Base;
using Managers;
using UnityEngine.Events;

namespace Inventory.View
{
    public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        public SlotType slotType;
        public int slotIndex;
        public UnityEvent onClickAction;
        public GameObject ammoUI;

        public void SetSlotType(SlotType type, int index)
        {
            slotType = type;
            slotIndex = index;
        }

        private void CheckExtraUI(ItemAmount itemAmount)
        {
            if (ammoUI != null)
            {
                if (itemAmount != null && itemAmount.SoItem != null && itemAmount.SoItem.AmmoType != null)
                {
                    ammoUI.SetActive(true);
                }
                else
                {
                    ammoUI.SetActive(false);
                }
            }
        }

        public void SetItem(ItemAmount itemAmount, InventoryItem fromItem = null)
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
                GameObject newItem = Instantiate(GameManager.Canvas.inventoryManager.itemSlotPrefab, transform);
                item = newItem.GetComponent<InventoryItem>();
            }
            item.SetItem(itemAmount, fromItem);

            CheckExtraUI(itemAmount);
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

            return GetItemAmount().SoItem;
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
            if (HandleInventoryToAmmo(fromSlot, fromItem, toItem)) return;
        }

        private bool HandleInventoryToInventory(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != GameManager.Canvas.inventoryManager.inventorySlotType ||
                slotType != GameManager.Canvas.inventoryManager.inventorySlotType)
                return false;

            fromItem.SetParent(transform);
            if (toItem)
            {
                toItem.SetParent(fromSlot.transform);
            }
            
            GameManager.Player.inventory.SwapItems(fromSlot.slotIndex, slotIndex);
            
            return true;
        }

        private bool HandleInventoryToToolbar(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem) //todo sacar el to item y hacerlo por toolbar esto
        {
            if (fromSlot.slotType != GameManager.Canvas.inventoryManager.inventorySlotType ||
                slotType != GameManager.Canvas.inventoryManager.toolbarSlotType)
                return false;

            if (!fromItem.itemAmount.SoItem.IsEquippable) return true;

            Toolbar toolbar = GameManager.Player.GetComponent<Toolbar>();
            toolbar.SetIndex(slotIndex, fromSlot.slotIndex);


            if (toItem != null && fromItem == toItem.originalItem) return true;

            if (toItem != null) toItem.originalItem.SetEquipable(-1);

            InventoryItem existingItem = GameManager.Canvas.inventoryManager.toolbarView.GetItem(fromItem);
            
            if (existingItem != null)
            {
                existingItem.GetComponentInParent<InventorySlot>().CheckExtraUI(new ItemAmount());
                Destroy(existingItem.gameObject);
            }

            if (toItem == null)
            {
                GameObject newItem = Instantiate(GameManager.Canvas.inventoryManager.itemSlotPrefab, transform);
                toItem = newItem.GetComponent<InventoryItem>();
            }

            SetItem(fromItem.itemAmount, fromItem);
            fromItem.SetEquipable(slotIndex);
            toItem.SetEquipable(slotIndex);
            GameManager.Canvas.inventoryManager.toolbarView.SetItemEquipped();
            return true;
        }

        private bool HandleToolbarToToolbar(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != GameManager.Canvas.inventoryManager.toolbarSlotType ||
                slotType != GameManager.Canvas.inventoryManager.toolbarSlotType)
                return false;

            fromItem.SetParent(transform);
            fromItem.SetEquipable(slotIndex);
            fromItem.originalItem.SetEquipable(slotIndex);
            CheckExtraUI(fromItem.itemAmount);
            if (toItem)
            {
                fromSlot.CheckExtraUI(toItem.itemAmount);
                toItem.SetParent(fromSlot.transform);
                toItem.SetEquipable(fromSlot.slotIndex);
                toItem.originalItem.SetEquipable(fromSlot.slotIndex);
            }
            else
            {
                fromSlot.CheckExtraUI(new ItemAmount());
            }
            Toolbar toolbar = GameManager.Player.GetComponent<Toolbar>();
            toolbar.SwapIndexes(slotIndex, fromSlot.slotIndex);

            GameManager.Canvas.inventoryManager.toolbarView.SetItemEquipped();
            return true;
        }

        private bool HandleToolbarToInventory(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != GameManager.Canvas.inventoryManager.toolbarSlotType ||
                slotType != GameManager.Canvas.inventoryManager.inventorySlotType)
                return false;
            
            Toolbar toolbar = GameManager.Player.GetComponent<Toolbar>();
            toolbar.SetIndex(toolbar.GetIndex(fromSlot.slotIndex), -1);

            fromItem.originalItem.SetEquipable(-1);
            fromSlot.CheckExtraUI(new ItemAmount());
            Destroy(fromItem.gameObject);
            if (GameManager.Canvas.inventoryManager.toolbarView.SelectedSlot.slotIndex == fromSlot.slotIndex)
            {
                ItemsInHand.Instance.SetItemEquipped(null);
            }

            return true;
        }
        
        private bool HandleInventoryToAmmo(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != GameManager.Canvas.inventoryManager.inventorySlotType ||
                slotType != GameManager.Canvas.inventoryManager.ammoSlotType)
                return false;
            
            return true;
        }
    }
}