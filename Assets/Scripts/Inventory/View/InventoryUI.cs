using Interfaces;
using Inventory.Model;
using Items.Base;
using UnityEngine;
using System.Collections.Generic;
using Inventory.Interfaces;

namespace Inventory.View
{
    public class InventoryUI : MonoBehaviour, IInventoryObserver
    {
        [SerializeField] protected SlotUI[] slots;
        [SerializeField] protected SlotType slotType;
        [SerializeField] private InventorySystem _inventory;

        protected virtual void Awake()
        {
            InitializeInventory();
        }

        private void InitializeInventory()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].SetSlotType(slotType, i);
            }
        }

        public void SetInventory(InventorySystem newInventory)
        {
            if (_inventory != null)
            {
                _inventory.RemoveObserver(this);
            }

            _inventory = newInventory;

            if (_inventory != null)
            {
                _inventory.AddObserver(this);
                OnInventoryChanged(_inventory.GetAllItems);
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