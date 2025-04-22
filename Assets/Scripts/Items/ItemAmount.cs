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
            return AddAmount(newAmount);
        }

        public int AddAmount(int amountAdded)
        {
            if (IsEmpty || amountAdded <= 0) return amountAdded;

            int add = Mathf.Min(StackSpace, amountAdded);
            amount += add;
            return amountAdded - add;
        }

        public int RemoveAmount(int amountRemoved)
        {
            if (IsEmpty || amountRemoved <= 0) return amountRemoved;

            int toRemove = Mathf.Min(amount, amountRemoved);
            amount -= toRemove;
            if (amount == 0) Clear();

            return amountRemoved - toRemove;
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