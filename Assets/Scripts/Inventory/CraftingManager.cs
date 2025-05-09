using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Inventory.View;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private InventorySlot[] _slots;
        [SerializeField] private int _selectedSlot = 0;
        [SerializeField] private InventorySlot _craftedSlot;
        [SerializeField] private GameObject slotSelector;
        [SerializeField] private SO_Item bullet;

        private void Awake()
        {
            ChangeSelectedSlot(_selectedSlot);
        }
        
        public void ChangeSelectedSlot(int slot)
        {
            if (_selectedSlot == slot) return;
            if (slot >= _slots.Length) return;

            _selectedSlot = slot;
            slotSelector.transform.SetParent(_slots[_selectedSlot].transform, false);
            slotSelector.transform.localPosition = Vector3.zero;
        }

        public void SetItem(ItemAmount itemAmount)
        {
            InventoryItem item = _slots[_selectedSlot].GetComponentInChildren<InventoryItem>();

            if (item == null)
            {
                GameObject itemobject = Instantiate(CanvasManager.Instance.inventoryManager.itemSlotPrefab, _slots[_selectedSlot].transform);
                item = itemobject.GetComponent<InventoryItem>();
                item.SetRaycast(false);
            }
            
            item.SetItem(itemAmount);
            ChangeSelectedSlot(_selectedSlot + 1);
            UpdateCrafting();
        }

        public void Clear()
        {
            foreach (var slot in _slots)
            {
                InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            ChangeSelectedSlot(0);
            _craftedSlot.SetItem(new ItemAmount(null, 0));
        }

        private void UpdateCrafting()
        {
            ItemAmount bullet = new ItemAmount(this.bullet, 1);
            if (_slots[0].GetItemObject() != null && _slots[1].GetItemObject() != null)
            {
                if (_slots[2].GetItemObject() != null)
                {
                    bullet.AddModifier(_slots[2].GetItemObject());
                    _craftedSlot.SetItem(bullet);
                }
                else
                {
                    if (_slots[0].GetItemObject() == _slots[1].GetItemObject() != _slots[2].GetItemObject())
                    {
                        bullet.AddModifier(_slots[0].GetItemObject());
                        _craftedSlot.SetItem(bullet);
                    }
                    else
                    {
                        _craftedSlot.SetItem(new ItemAmount(null, 0));
                    }
                }
            }
            else
            {
                _craftedSlot.SetItem(new ItemAmount(null, 0));
            }
        }

        public void CraftItem()
        {
            ItemAmount item = _craftedSlot.GetItemAmount();
            if (item.IsEmpty)
            {
                return;
            }
            PlayerController.Instance.inventory.AddItem(_craftedSlot.GetItemAmount());
        }
    }
}
