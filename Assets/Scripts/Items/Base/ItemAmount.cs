using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items.Base
{
    [Serializable]
    public class ItemAmount
    {
        [SerializeField] private int amount;
        [SerializeField] private SO_Item soItem;
        private LinkedList<ItemAmount> modifiers;
        private bool _overflow;
        private string _itemName;
        private string _description;
        
        public int Amount => amount;
        public SO_Item SoItem => soItem;
        public LinkedList<ItemAmount> Modifiers => modifiers ??= new LinkedList<ItemAmount>();
        public string ItemName => _itemName;
        public string Description => _description;
        
        public ItemAmount(SO_Item newSoItem = null, int newAmount = 0, LinkedList<ItemAmount> modifiers = null, bool overflow = false)
        {
            soItem = newSoItem;
            amount = newAmount;
            _overflow = overflow;
            this.modifiers = modifiers ?? new LinkedList<ItemAmount>();
        }
        
        public void SetOverflow(bool overflow = false)
        {
            _overflow = overflow;
        }
        
        public bool IsEmpty => soItem == null;
        public bool IsFull => soItem != null && !_overflow && amount >= Stack;
        public int Stack => soItem != null ? soItem.Stack : 0;

        public bool IsStackable(ItemAmount other)
        {
            if (other == null || IsFull || other.IsFull) return false;

            if (soItem != other.soItem) return false;

            if (Modifiers.Count != other.Modifiers.Count) return false;

            return Modifiers.All(mod => other.Modifiers.Any(m => m.soItem == mod.soItem));
        }

        public int SetItem(ItemAmount itemAmount)
        {
            soItem = itemAmount.SoItem;
            SetAmount(_overflow ? Mathf.Max(0, itemAmount.Amount) : Mathf.Clamp(itemAmount.Amount, 0, SoItem.Stack));
            modifiers = itemAmount.Modifiers;
            SetItemNameAndDescription();
            return _overflow ? 0 : Mathf.Max(0, itemAmount.Amount - SoItem.Stack);
        }
        
        public int SetAmount(int newAmount)
        {
            if (IsEmpty) return newAmount;

            int clampedAmount = _overflow ? Mathf.Max(0, newAmount) : Mathf.Clamp(newAmount, 0, SoItem.Stack);
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
            soItem = null;
            amount = 0;
            modifiers = new LinkedList<ItemAmount>();
        }

        public void AddModifier(ItemAmount itemAmount)
        {
            if (IsEmpty) return;

            if (modifiers.Contains(itemAmount)) return;

            var newPriority = itemAmount.SoItem.ModifierPriority;
            
            var current = modifiers.First;
            while (current != null)
            {
                if (current.Value.SoItem.ModifierPriority > newPriority)
                {
                    modifiers.AddBefore(current, itemAmount);
                    SetItemNameAndDescription();
                    return;
                }
                current = current.Next;
            }
            
            modifiers.AddLast(itemAmount);
            SetItemNameAndDescription();
        }

        private void SetItemNameAndDescription()
        {
            if (soItem == null)
            {
                _itemName = string.Empty;
                _description = string.Empty;
                return;
            }

            string baseName = soItem.ItemName;
            foreach (var modifier in modifiers)
            {
                baseName = modifier.soItem.ModifierName + " " + baseName;
            }

            _itemName = baseName;

            string desc = soItem.Description;
            if (modifiers != null && modifiers.Count > 0)
            {
                desc += "\nMade of:";
                foreach (var modifier in modifiers)
                {
                    desc += "\n- " + modifier.soItem.ItemName;
                }
            }

            _description = desc;
        }
    }
}
