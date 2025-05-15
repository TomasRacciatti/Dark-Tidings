using System.Collections.Generic;
using UnityEngine;

namespace Items.Base
{
    [System.Serializable]
    public class ItemInstance
    {
        [SerializeField] private SO_Item soItem; //for stack, mesh, materials, equipable, type
        [SerializeField] private string _itemName;
        [SerializeField] private string _description;
        [SerializeField] private List<SO_Item> _modifiers;

        // Propiedades públicas
        public SO_Item SoItem => soItem;
        public string ItemName => _itemName;
        public Sprite Image => soItem.Image;
        public string Description => _description;
        public List<SO_Item> Modifiers => _modifiers;
        public int Stack => soItem.Stack;
        public ItemType ItemType => soItem.ItemType;
        public bool IsEquippable => soItem.IsEquippable;
        public Mesh Mesh => soItem.Mesh;
        public Material[] Materials => soItem.Materials;
        
        public bool IsEmpty => soItem == null;

        public ItemInstance()
        {
            soItem = null;
            _itemName = string.Empty;
            _description = string.Empty;
            _modifiers = new List<SO_Item>();
        }
        
        public ItemInstance(SO_Item soItem, List<SO_Item> modifiers)
        {
            if (soItem == null)
            {
                this.soItem = null;
                _itemName = string.Empty;
                _description = string.Empty;
                _modifiers = new List<SO_Item>();
                return;
            }

            this.soItem = soItem;
            _itemName = soItem.ItemName;
            _description = soItem.Description;
            _modifiers = modifiers;
            foreach (var modifier in _modifiers)
            {
                _itemName = modifier.ModifierName + " " + _itemName;
            }
        }

        public bool IsStackable(ItemInstance itemInstance)
        {
            if (itemInstance == null) return false;
            if (soItem != itemInstance.SoItem) return false;
            if (_itemName != itemInstance.ItemName) return false;
            if (_description != itemInstance.Description) return false;

            // Comparar modificadores por contenido
            if (_modifiers.Count != itemInstance.Modifiers.Count) return false;
            for (int i = 0; i < _modifiers.Count; i++)
            {
                if (!_modifiers[i].Equals(itemInstance.Modifiers[i]))
                    return false;
            }
    
            return true;
        }

        public void AddModifier(SO_Item soItem)
        {
            _modifiers.Add(soItem);
            _itemName = soItem.ModifierName + " " + _itemName;
        }
    }
}