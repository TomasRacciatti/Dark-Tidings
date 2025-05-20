using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Base
{
    [Serializable]
    public class ItemAmount
    {
        [SerializeField] private int amount;
        [SerializeField] private SO_Item soItem; //for stack, mesh, materials, equipable, type
        [SerializeField] private List<ItemAmount> modifiers;
        private bool _overflow;
        
        public SO_Item Item => !IsEmpty ? soItem : null;
        public int Amount => amount;
        
        private string _itemName;
        private string _description;
        
        public SO_Item SoItem => soItem;
        public string ItemName => _itemName;
        public string Description => _description;
        public List<ItemAmount> Modifiers => modifiers ??= new List<ItemAmount>();
        
        public void SetOverflow(bool overflow = false)
        {
            _overflow = overflow;
        }

        public ItemAmount(SO_Item newSoItem = null, int newAmount = 0, List<ItemAmount> modifiers = null, bool overflow = false)
        {
            soItem = newSoItem;
            amount = newAmount;
            _overflow = overflow;
            this.modifiers = modifiers ?? new List<ItemAmount>();
        }
        
        public bool IsEmpty => SoItem == null;
        public bool IsFull => SoItem != null && !_overflow && amount >= Stack;
        public int Stack => SoItem != null ? SoItem.Stack : 0;
        public SO_Item GetSoItem => SoItem;

        public bool IsStackable(ItemAmount other)
        {
            if (other == null || this.IsFull || other.IsFull) return false;

            if (this.soItem != other.soItem) return false;

            if (this.modifiers.Count != other.modifiers.Count) return false;
            
            var thisSet = new HashSet<ItemAmount>(this.modifiers);
            var otherSet = new HashSet<ItemAmount>(other.modifiers);

            return thisSet.SetEquals(otherSet);
        }

        public int SetItem(ItemAmount itemAmount)
        {
            soItem = itemAmount.SoItem;

            SetAmount(_overflow ? Mathf.Max(0, itemAmount.Amount) : Mathf.Clamp(itemAmount.Amount, 0, SoItem.Stack));
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
            modifiers = new List<ItemAmount>();
        }

        public void AddModifier(ItemAmount itemAmount)
        {
            if (IsEmpty) return;
            
            if (modifiers.Contains(itemAmount)) return;

            var newPriority = itemAmount.SoItem.ModifierPriority;

            int index = modifiers.FindIndex(m => 
                m.SoItem.ModifierPriority > newPriority
            );

            if (index >= 0)
                modifiers.Insert(index, itemAmount);
            else
                modifiers.Add(itemAmount);

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

        public bool Equals(ItemAmount other)
        {
            throw new NotImplementedException();
        }
    }
}
