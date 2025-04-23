using UnityEngine;
using Items;

namespace Inventory
{
    [System.Serializable]
    public struct ItemAmount
    {
        public ItemObject item;
        public int amount;

        public ItemAmount(ItemObject newItem, int newAmount) : this()
        {
            SetItem(newItem, newAmount);
        }

        public bool IsEmpty => item == null;
        public bool IsFull => item != null && amount >= item.stack;
        public int StackSpace => item != null ? item.stack - amount : 0;

        public bool CanStackWith(ItemObject other)
        {
            return item == other && !IsFull;
        }

        public int SetItem(ItemObject newItem, int newAmount)
        {
            if (newItem == null || newAmount <= 0)
            {
                Clear();
                return newAmount;
            }

            item = newItem;
            SetAmount(Mathf.Min(newAmount, item.stack));
            return Mathf.Max(0, newAmount - item.stack);
        }

        public int SetAmount(int newAmount)
        {
            if (IsEmpty) return newAmount;
            int clampedAmount = Mathf.Clamp(newAmount, 0, item.stack);
            amount = clampedAmount;
            if (amount <= 0) Clear();
            return newAmount - clampedAmount;
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
            item = null;
            amount = 0;
        }

        public string ItemToString()
        {
            return IsEmpty ? "Empty" : $"{item.name} x{amount}";
        }
    }
}