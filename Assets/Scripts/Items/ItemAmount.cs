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
        public int Amount => _amount;

        public ItemAmount(ItemObject newItem = null, int newAmount = 0, bool allowOverflow = false)
        {
            _itemInstance = new ItemInstance(newItem);
            _amount = newAmount;
            _allowOverflow = allowOverflow;
        }
        
        public bool IsEmpty => _itemInstance == null ||_itemInstance.ItemObject == null;
        public bool IsFull => _itemInstance != null && _itemInstance.ItemObject != null && !_allowOverflow && _amount >= _itemInstance.Stack;
        public int Stack => _itemInstance.ItemObject != null ? _itemInstance.Stack : 0;
        public ItemObject GetItemObject => _itemInstance.ItemObject;

        public void SetOverflow(bool allowOverflow = false)
        {
            _allowOverflow = allowOverflow;
        }

        public bool IsStackable(ItemAmount itemAmount)
        {
            return !IsFull && _itemInstance.ItemObject != null && _itemInstance.IsStackable(itemAmount.ItemInstance);
        }

        public int SetItem(ItemAmount itemAmount)
        {
            _itemInstance = new ItemInstance(itemAmount.GetItemObject);

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
            _itemInstance = new ItemInstance(null);
            _amount = 0;
        }

        public string ItemToString()
        {
            return IsEmpty ? "Empty" : $"{_itemInstance.ItemName} x{_amount}";
        }
    }
}
