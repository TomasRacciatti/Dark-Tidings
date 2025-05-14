using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Inventory
{
    [System.Serializable]
    public struct ItemAmount
    {
        [SerializeField] private ItemInstance _itemInstance;
        [SerializeField] private int _amount;
        private bool _allowOverflow;

        public ItemInstance ItemInstance => _itemInstance;
        public SO_Item Item => !IsEmpty ? _itemInstance.SoItem : null;
        public int Amount => _amount;

        public ItemAmount(SO_Item newSoItem = null, int newAmount = 0, bool allowOverflow = false)
        {
            _itemInstance = new ItemInstance(newSoItem, new List<SO_Item>());
            _amount = newAmount;
            _allowOverflow = allowOverflow;
        }
        
        public bool IsEmpty => _itemInstance == null ||_itemInstance.SoItem == null;
        public bool IsFull => _itemInstance != null && _itemInstance.SoItem != null && !_allowOverflow && _amount >= _itemInstance.Stack;
        public int Stack => _itemInstance.SoItem != null ? _itemInstance.Stack : 0;
        public SO_Item GetSoItem => _itemInstance.SoItem;

        public void SetOverflow(bool allowOverflow = false)
        {
            _allowOverflow = allowOverflow;
        }

        public bool IsStackable(ItemAmount itemAmount)
        {
            return !IsFull && _itemInstance.SoItem != null && _itemInstance.IsStackable(itemAmount.ItemInstance);
        }

        public int SetItem(ItemAmount itemAmount)
        {
            _itemInstance = new ItemInstance(itemAmount.GetSoItem, itemAmount.ItemInstance.Modifiers);

            _amount = _allowOverflow ? Mathf.Max(0, itemAmount.Amount) : Mathf.Clamp(itemAmount.Amount, 0, _itemInstance.Stack);
            return _allowOverflow ? 0 : Mathf.Max(0, itemAmount.Amount - _itemInstance.Stack);
        }

        public int SetAmount(int newAmount)
        {
            if (IsEmpty) return newAmount;

            int clampedAmount = _allowOverflow ? Mathf.Max(0, newAmount) : Mathf.Clamp(newAmount, 0, _itemInstance.Stack);
            _amount = clampedAmount;

            if (_amount <= 0) Clear();
            return _allowOverflow ? 0 : newAmount - clampedAmount;
        }

        public int AddAmount(int amountToAdd)
        {
            if (IsEmpty || amountToAdd <= 0) return amountToAdd;
            return SetAmount(_amount + amountToAdd);
        }

        public int RemoveAmount(int amountToRemove)
        {
            if (IsEmpty || amountToRemove <= 0) return amountToRemove;
            return SetAmount(_amount - amountToRemove);
        }

        public void Clear()
        {
            _itemInstance = new ItemInstance(null, new List<SO_Item>());
            _amount = 0;
        }

        public void AddModifier(SO_Item soItem)
        {
            _itemInstance.AddModifier(soItem);
        }

        public string ItemToString()
        {
            return IsEmpty ? "Empty" : $"{_itemInstance.ItemName} x{_amount}";
        }
    }
}
