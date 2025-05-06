using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace Items
{
    [System.Serializable]
    public class ItemInstance
    {
        [SerializeField] private ItemObject _itemObject; //for stack, mesh, materials, equipable, type
        [SerializeField] private string _itemName;
        [SerializeField] private string _description;
        [SerializeField] private List<ItemObject> _modifiers;

        // Propiedades públicas
        public ItemObject ItemObject => _itemObject;
        public string ItemName => _itemName;
        public Sprite Image => _itemObject.Image;
        public string Description => _description;
        public List<ItemObject> Modifiers => _modifiers;
        public int Stack => _itemObject.Stack;
        public ItemType ItemType => _itemObject.ItemType;
        public bool IsEquippable => _itemObject.IsEquippable;
        public Mesh Mesh => _itemObject.Mesh;
        public Material[] Materials => _itemObject.Materials;
        
        public bool IsEmpty => _itemObject == null;

        public ItemInstance()
        {
            _itemObject = null;
            _itemName = string.Empty;
            _description = string.Empty;
            _modifiers = new List<ItemObject>();
        }
        
        public ItemInstance(ItemObject itemObject, List<ItemObject> modifiers)
        {
            if (itemObject == null)
            {
                _itemObject = null;
                _itemName = string.Empty;
                _description = string.Empty;
                _modifiers = new List<ItemObject>();
                return;
            }

            _itemObject = itemObject;
            _itemName = itemObject.ItemName;
            _description = itemObject.Description;
            _modifiers = modifiers;
            foreach (var modifier in _modifiers)
            {
                _itemName = modifier.ModifierName + " " + _itemName;
            }
        }

        public bool IsStackable(ItemInstance itemInstance)
        {
            if (itemInstance == null) return false;
            if (_itemObject != itemInstance.ItemObject) return false;
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

        public void AddModifier(ItemObject itemObject)
        {
            _modifiers.Add(itemObject);
            _itemName = itemObject.ModifierName + " " + _itemName;
        }
    }
}