using UnityEngine;
using UnityEngine.EventSystems;
using Characters.Player;
using Inventory.Controller;
using Items.Base;
using Managers;
using UnityEngine.Events;

namespace Inventory.View
{
    public class SlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler
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

        public void SetItem(ItemAmount itemAmount, ItemUI originalItem = null)
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
            itemUI.SetItem(itemAmount, originalItem);

            CheckExtraUI(itemAmount);
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

        public void OnPointerClick(PointerEventData eventData)
        {
            onClickAction?.Invoke();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemUI fromItemUI = eventData.pointerDrag.GetComponent<ItemUI>();
            if (fromItemUI == null)
            {
                transform.localPosition = Vector3.zero;
                return;
            }

            SlotUI fromSlotUI = fromItemUI.GetComponentInParent<SlotUI>();
            ItemUI toItemUI = GetComponentInChildren<ItemUI>();
            
            if (HandleInventoryToInventory(fromSlotUI, fromItemUI, toItemUI)) return;
            if (HandleInventoryToToolbar(fromSlotUI, fromItemUI, toItemUI)) return;
            if (HandleToolbarToToolbar(fromSlotUI, fromItemUI, toItemUI)) return;
            if (HandleToolbarToInventory(fromSlotUI, fromItemUI, toItemUI)) return;
            //if (HandleInventoryToAmmo(fromSlotUI, fromItemUI, toItemUI)) return;
        }

        private bool HandleInventoryToInventory(SlotUI fromSlotUI, ItemUI fromItemUI, ItemUI toItemUI)
        {
            if (fromSlotUI.slotType != GameManager.Canvas.inventoryManager.inventorySlotType ||
                slotType != GameManager.Canvas.inventoryManager.inventorySlotType)
                return false;

            fromItemUI.SetParent(transform);
            if (toItemUI)
            {
                toItemUI.SetParent(fromSlotUI.transform);
            }
            
            GameManager.Player.inventory.SwapItems(fromSlotUI.slotIndex, slotIndex);
            
            return true;
        }

        private bool HandleInventoryToToolbar(SlotUI fromSlotUI, ItemUI fromItemUI, ItemUI toItemUI) //todo sacar el to item y hacerlo por toolbar esto
        {
            if (fromSlotUI.slotType != GameManager.Canvas.inventoryManager.inventorySlotType ||
                slotType != GameManager.Canvas.inventoryManager.toolbarSlotType)
                return false;

            if (!fromItemUI.itemAmount.SoItem.IsEquippable) return true;

            Toolbar toolbar = GameManager.Player.GetComponent<Toolbar>();
            toolbar.SetIndex(slotIndex, fromSlotUI.slotIndex);


            if (toItemUI != null && fromItemUI == toItemUI.originalItemUI) return true;

            if (toItemUI != null) toItemUI.originalItemUI.SetEquipable(-1);

            ItemUI existingItemUI = GameManager.Canvas.inventoryManager.toolbarUI.GetItem(fromItemUI);
            
            if (existingItemUI != null)
            {
                existingItemUI.GetComponentInParent<SlotUI>().CheckExtraUI(new ItemAmount());
                Destroy(existingItemUI.gameObject);
            }

            if (toItemUI == null)
            {
                GameObject newItem = Instantiate(GameManager.Canvas.inventoryManager.itemSlotPrefab, transform);
                toItemUI = newItem.GetComponent<ItemUI>();
            }

            SetItem(fromItemUI.itemAmount, fromItemUI);
            fromItemUI.SetEquipable(slotIndex);
            toItemUI.SetEquipable(slotIndex);
            GameManager.Canvas.inventoryManager.toolbarUI.SetItemEquipped();
            return true;
        }

        private bool HandleToolbarToToolbar(SlotUI fromSlotUI, ItemUI fromItemUI, ItemUI toItemUI)
        {
            if (fromSlotUI.slotType != GameManager.Canvas.inventoryManager.toolbarSlotType ||
                slotType != GameManager.Canvas.inventoryManager.toolbarSlotType)
                return false;

            fromItemUI.SetParent(transform);
            fromItemUI.SetEquipable(slotIndex);
            fromItemUI.originalItemUI.SetEquipable(slotIndex);
            CheckExtraUI(fromItemUI.itemAmount);
            if (toItemUI)
            {
                fromSlotUI.CheckExtraUI(toItemUI.itemAmount);
                toItemUI.SetParent(fromSlotUI.transform);
                toItemUI.SetEquipable(fromSlotUI.slotIndex);
                toItemUI.originalItemUI.SetEquipable(fromSlotUI.slotIndex);
            }
            else
            {
                fromSlotUI.CheckExtraUI(new ItemAmount());
            }
            Toolbar toolbar = GameManager.Player.GetComponent<Toolbar>();
            toolbar.SwapIndexes(slotIndex, fromSlotUI.slotIndex);

            GameManager.Canvas.inventoryManager.toolbarUI.SetItemEquipped();
            return true;
        }

        private bool HandleToolbarToInventory(SlotUI fromSlotUI, ItemUI fromItemUI, ItemUI toItemUI)
        {
            if (fromSlotUI.slotType != GameManager.Canvas.inventoryManager.toolbarSlotType ||
                slotType != GameManager.Canvas.inventoryManager.inventorySlotType)
                return false;
            
            Toolbar toolbar = GameManager.Player.GetComponent<Toolbar>();
            toolbar.SetIndex(toolbar.GetIndex(fromSlotUI.slotIndex), -1);

            fromItemUI.originalItemUI.SetEquipable(-1);
            fromSlotUI.CheckExtraUI(new ItemAmount());
            Destroy(fromItemUI.gameObject);
            if (GameManager.Canvas.inventoryManager.toolbarUI.SelectedSlotUI.slotIndex == fromSlotUI.slotIndex)
            {
                ItemsInHand.Instance.SetItemEquipped(null);
            }

            return true;
        }
        
        private bool HandleInventoryToAmmo(SlotUI fromSlotUI, ItemUI fromItemUI, ItemUI toItemUI)
        {
            if (fromSlotUI.slotType != GameManager.Canvas.inventoryManager.inventorySlotType ||
                slotType != GameManager.Canvas.inventoryManager.ammoSlotType)
                return false;
            
            return true;
        }
    }
}