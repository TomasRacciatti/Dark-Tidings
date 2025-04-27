using UnityEngine;
using Items;
using UnityEngine.Serialization;

namespace Inventory
{
    [System.Serializable]
    public struct ItemAmount
    {
        //[SerializeField] private ItemObject _itemObject;
        [SerializeField] private Item _item;
        [SerializeField] private int _amount;

        // Getters en una línea utilizando expresión lambda
        public Item Item => _item;
        public int Amount => _amount;

        public ItemAmount(ItemObject newItem, int newAmount) : this()
        {
            InitializeItem(newItem, newAmount);
        }

        public bool IsEmpty => _item == null;
        public bool IsFull => _item != null && _amount >= _item.Stack;
        public int StackSpace => _item?.Stack - _amount ?? 0;
        public ItemObject GetItemObject => _item.ItemObject;

        public bool IsStackable(ItemAmount itemAmount)
        {
            return !IsFull && _item.IsStackable(itemAmount.Item);
        }

        public int InitializeItem(ItemObject newItem, int newAmount)
        {
            if (newItem == null || newAmount <= 0) //limpia por las dudas
            {
                Clear();
                return newAmount;
            }
            
            _item.InitializeItem(newItem);

            SetAmount(Mathf.Min(newAmount, _item.Stack));
            return Mathf.Max(0, newAmount - _item.Stack);
        }

        public int SetItem(ItemAmount itemAmount)
        {
            if (_item != itemAmount.Item && !IsStackable(itemAmount))
            {
                Clear();
            }

            _item = itemAmount.Item;
            return SetAmount(itemAmount.Amount);
        }

        public int SetAmount(int newAmount)
        {
            if (IsEmpty) return newAmount;
            int clampedAmount = Mathf.Clamp(newAmount, 0, _item.Stack);
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
            return IsEmpty ? "Empty" : $"{_item.ItemName} x{_amount}";
        }
    }
}