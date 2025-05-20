using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Base
{
    [System.Serializable]
    public class ItemInstance
    {
        [SerializeField] private SO_Item soItem; //for stack, mesh, materials, equipable, type
        [SerializeField] private List<ItemAmount> modifiers;
        
        private string _itemName;
        private string _description;

        // Propiedades públicas
        public SO_Item SoItem => soItem;
        public string ItemName => _itemName;
        public string Description => _description;
        public List<ItemAmount> Modifiers => modifiers ??= new List<ItemAmount>();

        public ItemInstance(SO_Item soItem = null, List<ItemAmount> modifiers = null)
        {
            this.soItem = soItem;

            if (soItem == null)
            {
                _itemName = string.Empty;
                _description = string.Empty;
                this.modifiers = new List<ItemAmount>();
                return;
            }

            this.modifiers = modifiers != null ? new List<ItemAmount>(modifiers) : new List<ItemAmount>();

            SetItemNameAndDescription();
        }

        public bool IsStackable(ItemInstance other)
        {
            if (other == null || soItem != other.soItem) return false;

            if (modifiers.Count != other.modifiers.Count) return false;

            var thisSet = new HashSet<ItemAmount>(modifiers);
            var otherSet = new HashSet<ItemAmount>(other.modifiers);

            return thisSet.SetEquals(otherSet);
        }

        public void AddModifier(ItemAmount itemAmount)
        {
            if (itemAmount?.ItemInstance == null) return;
            
            if (modifiers.Contains(itemAmount)) return;

            var newPriority = itemAmount.ItemInstance.SoItem.ModifierPriority;

            int index = modifiers.FindIndex(m => 
                m.ItemInstance.SoItem.ModifierPriority > newPriority
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
                baseName = modifier.ItemInstance.soItem.ModifierName + " " + baseName;
            }

            _itemName = baseName;

            string desc = soItem.Description;
            if (modifiers != null && modifiers.Count > 0)
            {
                desc += "\nMade of:";
                foreach (var modifier in modifiers)
                {
                    desc += "\n- " + modifier.ItemInstance.soItem.ItemName;
                }
            }

            _description = desc;
        }
    }
}