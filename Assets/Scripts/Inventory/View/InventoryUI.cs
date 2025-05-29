using System;
using Interfaces;
using Inventory.Model;
using Items.Base;
using UnityEngine;
using System.Collections.Generic;
using Inventory.Interfaces;
using Managers;
using UnityEngine.Serialization;

namespace Inventory.View
{
    public class InventoryUI : MonoBehaviour, IInventoryObserver
    {
        [SerializeField] protected SlotUI[] slots;
        [SerializeField] protected InventorySystem inventory;
        
        public InventorySystem Inventory => inventory;

        private void Awake()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Initialize(this, i);
            }
        }

        protected virtual void Start()
        {
            if (inventory == null) inventory = GameManager.Player.inventory;
            SetInventory(inventory);
        }

        public void SetInventory(InventorySystem newInventory)
        {
            if (inventory != null)
            {
                inventory.RemoveObserver(this);
            }

            inventory = newInventory;

            if (inventory != null)
            {
                inventory.AddObserver(this);
                OnInventoryChanged(inventory.items);
            }
        }

        public void OnItemChanged(int index, ItemAmount item)
        {
            if (index < 0 || index >= slots.Length) return;
            slots[index].SetItem(item);
        }

        public void OnInventoryChanged(List<ItemAmount> allItems)
        {
            for (int i = 0; i < slots.Length && i < allItems.Count; i++)
            {
                slots[i].SetItem(allItems[i]);
            }
        }
    }
}