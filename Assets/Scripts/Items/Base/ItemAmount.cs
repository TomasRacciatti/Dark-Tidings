using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items.Base
{
    [Serializable]
    public class ItemAmount : ISerializationCallbackReceiver
    {
        [SerializeField] private int amount;
        [SerializeField] private SO_Item soItem;
        [NonSerialized] private List<ItemAmount> _modifiers;
        private bool _overflow;
        private string _itemName;
        private string _description;

        public int Amount => amount;
        public SO_Item SoItem => soItem;
        public List<ItemAmount> Modifiers => _modifiers ??= new List<ItemAmount>();
        public List<ItemAmount> CopyModifiers => 
            (_modifiers ??= new List<ItemAmount>())
            .Select(m => new ItemAmount(m))
            .ToList();
        public bool Overflow => _overflow;
        public string ItemName => _itemName;
        public string Description => _description;
        
        public ItemAmount(ItemAmount newItemAmount)
        {
            soItem = newItemAmount.SoItem;
            amount = newItemAmount.Amount;
            _modifiers = newItemAmount.Modifiers;
            SetItemNameAndDescription();
        }
        
        public ItemAmount(SO_Item newSoItem = null, int newAmount = 0, List<ItemAmount> modifiers = null, bool overflow = false)
        {
            soItem = newSoItem;
            amount = newAmount;
            _overflow = overflow;
            _modifiers = modifiers ?? new List<ItemAmount>();
            SetItemNameAndDescription();
        }

        public void SetOverflow(bool overflow = false)
        {
            _overflow = overflow;
        }
        
        public bool IsEmpty => soItem == null;
        public bool IsFull => soItem != null && !_overflow && amount >= Stack;
        public int Stack => soItem != null ? soItem.Stack : 0;

        public int SetItem(ItemAmount itemAmount)
        {
            soItem = itemAmount.SoItem;
            SetAmount(_overflow ? Mathf.Max(0, itemAmount.Amount) : Mathf.Clamp(itemAmount.Amount, 0, SoItem.Stack));
            _modifiers = itemAmount.Modifiers;
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
            _modifiers = new List<ItemAmount>();
        }

        public void AddModifier(ItemAmount itemAmount)
        {
            if (IsEmpty) return;

            //if (modifiers.Contains(itemAmount)) return;

            int newPriority = itemAmount.SoItem.ModifierPriority;
            
            int insertIndex = _modifiers.FindIndex(m => m.SoItem.ModifierPriority > newPriority);

            if (insertIndex >= 0)
                _modifiers.Insert(insertIndex, itemAmount);
            else
                _modifiers.Add(itemAmount);

            SetItemNameAndDescription();
        }

        public void SetItemNameAndDescription()
        {
            if (soItem == null)
            {
                _itemName = string.Empty;
                _description = string.Empty;
                return;
            }

            string baseName = soItem.ItemName;
            if (_modifiers != null && _modifiers.Count > 0)
            {
                foreach (var modifier in _modifiers)
                {
                    baseName = modifier.soItem.ModifierName + " " + baseName;
                }
            }

            _itemName = baseName;

            string desc = soItem.Description;
            if (_modifiers != null && _modifiers.Count > 0)
            {
                desc += "\nMade of:";
                foreach (var modifier in _modifiers)
                {
                    desc += "\n- " + modifier.soItem.ItemName;
                }
            }

            _description = desc;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            SetItemNameAndDescription();
        }
    }
}
