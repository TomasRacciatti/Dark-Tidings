using System;
using System.Collections.Generic;
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
            ItemsInHand.SetItemEquipped(inventorySystem.Items[selectedSlot].SoItem);
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
            ItemsInHand.SetItemEquipped(inventorySystem.Items[selectedSlot].SoItem);
            GameManager.Canvas.inventoryManager.toolbarUI.ChangeSelectedSlot(selectedSlot);
        }

        public void OnInventoryChanged(List<ItemAmount> currentItems)
        {
            ItemAmount item = currentItems[selectedSlot];
            if (item == null) return;
            ItemsInHand.SetItemEquipped(item.SoItem);
        }

        public void OnItemChanged(int index, ItemAmount newItem)
        {
            if (!inventorySystem.ValidIndex(index)) return;
            if (index != selectedSlot) return;
            
            ItemsInHand.SetItemEquipped(newItem.SoItem);
        }
    }
}