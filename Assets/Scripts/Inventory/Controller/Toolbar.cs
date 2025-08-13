using System;
using System.Collections.Generic;
using Effects;
using Inventory.Interfaces;
using Inventory.Model;
using Items.Base;
using Managers;
using UnityEngine;

namespace Inventory.Controller
{
    public class Toolbar : MonoBehaviour, IInventoryObserver
    {
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private InventorySystem inventorySystem;

        
        private void Awake()
        {
            if (inventorySystem == null) inventorySystem = GetComponent<InventorySystem>();
        }

        private void Start()
        {
            inventorySystem = InventoryUtility.SetInventoryObserver(null, inventorySystem, this);
            ItemsInHand.Instance.SetItemEquipped(inventorySystem.Items[selectedSlot].SoItem);
            Invoke(nameof(HideToolbar), 5);
        }

        public ItemAmount GetItem()
        {
            if (inventorySystem == null) return new ItemAmount();
            return inventorySystem.GetItemByIndex(selectedSlot);
        }
        
        public void SetSelectedSlot(int index)
        {
            if (index == selectedSlot) return;
            if (!inventorySystem.ValidIndex(index)) return;
            selectedSlot = index;
            ItemsInHand.Instance.SetItemEquipped(inventorySystem.Items[selectedSlot].SoItem);
            GameManager.Canvas.inventoryManager.toolbarUI.ChangeSelectedSlot(selectedSlot);
            
            if (GameManager.Canvas.inventoryManager.GetIndexInventory() == -1)
            {
                GameManager.Canvas.toolbarUI.Show();
                
                CancelInvoke(nameof(HideToolbar));
                Invoke(nameof(HideToolbar), 5);
            }
        }

        public void HideToolbar()
        {
            if (GameManager.Canvas.inventoryManager.GetIndexInventory() == -1)
            {
                GameManager.Canvas.toolbarUI.Hide();
            }
        }

        public void OnInventoryChanged(List<ItemAmount> currentItems)
        {
            ItemAmount item = currentItems[selectedSlot];
            if (item == null) return;
            ItemsInHand.Instance.SetItemEquipped(item.SoItem);
        }

        public void OnItemChanged(int index, ItemAmount newItem)
        {
            if (!inventorySystem.ValidIndex(index)) return;
            if (index != selectedSlot) return;
            
            ItemsInHand.Instance.SetItemEquipped(newItem.SoItem);
        }
    }
}