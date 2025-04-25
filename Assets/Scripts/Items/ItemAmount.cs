using UnityEngine;
using Items;

namespace Inventory
{
    [System.Serializable]
    public struct ItemAmount
    {
        [SerializeField] private ItemObject _item;
        [SerializeField] private int _amount;

        // Getters en una línea utilizando expresión lambda
        public ItemObject Item => _item;
        public int Amount => _amount;

        public ItemAmount(ItemObject newItem, int newAmount) : this()
        {
            SetItem(newItem, newAmount);
        }

        public bool IsEmpty => _item == null;
        public bool IsFull => _item != null && _amount >= _item.GetStack();
        public int StackSpace => _item != null ? _item.GetStack() - _amount : 0;

        public bool CanStackWith(ItemObject other)
        {
            return _item == other && !IsFull;
        }

        public int SetItem(ItemObject newItem, int newAmount)
        {
            if (newItem == null || newAmount <= 0)
            {
                Clear();
                return newAmount;
            }

            _item = newItem;
            SetAmount(Mathf.Min(newAmount, _item.GetStack()));
            return Mathf.Max(0, newAmount - _item.GetStack());
        }

        public int SetAmount(int newAmount)
        {
            if (IsEmpty) return newAmount;
            int clampedAmount = Mathf.Clamp(newAmount, 0, _item.GetStack());
            _amount = clampedAmount;
            if (_amount <= 0) Clear();
            return newAmount - clampedAmount;
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
            _item = null;
            _amount = 0;
        }

        public string ItemToString()
        {
            return IsEmpty ? "Empty" : $"{_item.name} x{_amount}";
        }
    }
}