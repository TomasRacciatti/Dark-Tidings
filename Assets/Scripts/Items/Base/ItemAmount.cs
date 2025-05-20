using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Base
{
    [Serializable]
    public class ItemAmount : IEquatable<ItemAmount>
    {
        [SerializeField] private int amount;
        [SerializeField] private ItemInstance itemInstance;
        private bool _overflow;

        public ItemInstance ItemInstance => itemInstance;
        public SO_Item Item => !IsEmpty ? itemInstance.SoItem : null;
        public int Amount => amount;
        public void SetOverflow(bool overflow = false)
        {
            _overflow = overflow;
        }

        public ItemAmount(SO_Item newSoItem = null, int newAmount = 0, List<ItemAmount> itemAmount = null, bool overflow = false)
        {
            itemInstance = new ItemInstance(newSoItem, itemAmount ?? new List<ItemAmount>());
            amount = newAmount;
            _overflow = overflow;
        }
        
        public bool IsEmpty => itemInstance == null || itemInstance.SoItem == null;
        public bool IsFull => itemInstance != null && itemInstance.SoItem != null && !_overflow && amount >= itemInstance.SoItem.Stack;
        public int Stack => itemInstance.SoItem != null ? itemInstance.SoItem.Stack : 0;
        public SO_Item GetSoItem => itemInstance.SoItem;


        public bool IsStackable(ItemAmount itemAmount)
        {
            return !IsFull && itemInstance.SoItem != null && itemInstance.IsStackable(itemAmount.ItemInstance);
        }

        public int SetItem(ItemAmount itemAmount)
        {
            itemInstance = itemAmount.ItemInstance;

            SetAmount(_overflow ? Mathf.Max(0, itemAmount.Amount) : Mathf.Clamp(itemAmount.Amount, 0, itemInstance.SoItem.Stack));
            return _overflow ? 0 : Mathf.Max(0, itemAmount.Amount - itemInstance.SoItem.Stack);
        }
        
        public int SetAmount(int newAmount)
        {
            if (IsEmpty) return newAmount;

            int clampedAmount = _overflow ? Mathf.Max(0, newAmount) : Mathf.Clamp(newAmount, 0, itemInstance.SoItem.Stack);
            amount = clampedAmount;

            if (amount <= 0) Clear();
            return _overflow ? 0 : newAmount - clampedAmount;
        }
        
        public int AddAmount(int amountToAdd)
        {
            if (IsEmpty || amountToAdd <= 0) return amountToAdd;
            return SetAmount(amount + amountToAdd);
        }

        public int RemoveAmount(int amountToRemove)
        {
            if (IsEmpty || amountToRemove <= 0) return amountToRemove;
            return SetAmount(amount - amountToRemove);
        }

        public void Clear()
        {
            itemInstance = new ItemInstance(null, new List<ItemAmount>());
            amount = 0;
        }

        public void AddModifier(ItemAmount itemAmount)
        {
            //itemInstance.AddModifier(itemAmount);
        }

        public string ItemToString()
        {
            return IsEmpty ? "Empty" : $"{itemInstance.ItemName} x{amount}";
        }

        public bool Equals(ItemAmount other)
        {
            return Equals(itemInstance, other.itemInstance) && amount == other.amount && _overflow == other._overflow;
        }
    }
}
