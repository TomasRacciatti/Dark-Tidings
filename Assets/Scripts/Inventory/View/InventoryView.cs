using Interfaces;
using Inventory.Model;
using Items.Base;
using UnityEngine;

namespace Inventory.View
{
    public class InventoryView : MonoBehaviour, IInventoryListener
    {
        [SerializeField] protected InventorySlot[] slots;
        [SerializeField] protected SlotType slotType;
        [SerializeField] private InventorySystem _inventory;
        [SerializeField] private GameObject inventorySlotPrefab;

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

        public void ItemUpdated(int index, ItemAmount itemAmount)
        {
            if (index < 0 || index >= slots.Length) return;

            slots[index].SetItem(itemAmount);
        }
        
        private void InventoryUpdated()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                ItemUpdated(i, _inventory.GetIndexItem(i));
            }
        }
        
        public void SetInventory(InventorySystem newInventory)
        {
            if (_inventory != null)
            {
                
                _inventory.OnItemUpdated -= ItemUpdated;
                _inventory.OnInventoryUpdated -= InventoryUpdated;
            }

            _inventory = newInventory;

            if (_inventory != null)
            {
                _inventory.OnItemUpdated += ItemUpdated;
                _inventory.OnInventoryUpdated += InventoryUpdated;
                InventoryUpdated();
            }
        }

        public void OnItemUpdated(int index, ItemAmount itemAmount)
        {
            if (index < 0 || index >= slots.Length) return;

            slots[index].SetItem(itemAmount);
        }

        public void OnInventoryUpdated()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                ItemUpdated(i, _inventory.GetIndexItem(i));
            }
        }
    }
}