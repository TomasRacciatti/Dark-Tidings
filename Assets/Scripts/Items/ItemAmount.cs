using UnityEngine;
using Items;

namespace Inventory
{
    [System.Serializable]
    public struct ItemAmount
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _amount;
        private bool _allowOverflow;

        public Item Item => _item;
        public int Amount => _amount;

        public ItemAmount(ItemObject newItem = null, int newAmount = 0, bool allowOverflow = false)
        {
            _item = new Item(newItem);
            _amount = newAmount;
            _allowOverflow = allowOverflow;
        }
        
        public bool IsEmpty => _item.ItemObject == null;
        public bool IsFull => _item.ItemObject != null && !_allowOverflow && _amount >= _item.Stack;
        public int StackSpace => _item?.Stack - _amount ?? 0;
        public ItemObject GetItemObject => _item.ItemObject;

        public void SetOverflow(bool allowOverflow = false)
        {
            _allowOverflow = allowOverflow;
        }

        public bool IsStackable(ItemAmount itemAmount)
        {
            return !IsFull && _item.ItemObject != null && _item.IsStackable(itemAmount.Item);
        }

        public int SetItem(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty)
            {
                Clear();
                return 0;
            }
            _item = new Item(itemAmount.GetItemObject);

            _amount = _allowOverflow ? Mathf.Max(0, itemAmount.Amount) : Mathf.Clamp(itemAmount.Amount, 0, _item.Stack);
            return _allowOverflow ? 0 : Mathf.Max(0, itemAmount.Amount - _item.Stack);
        }

        public int SetAmount(int newAmount)
        {
            if (IsEmpty) return newAmount;

            int clampedAmount = _allowOverflow ? Mathf.Max(0, newAmount) : Mathf.Clamp(newAmount, 0, _item.Stack);
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
            _item = new Item();
            _amount = 0;
        }

        public string ItemToString()
        {
            return IsEmpty ? "Empty" : $"{_item.ItemName} x{_amount}";
        }
    }
}
