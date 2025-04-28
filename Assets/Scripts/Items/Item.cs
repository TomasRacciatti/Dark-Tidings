using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    //[System.Serializable]
    public class Item
    {
        private ItemObject _itemObject; //for stack, mesh, materials, equipable, type
        private string _itemName;
        private string _description;
        private List<ItemModifier> _modifiers;

        // Propiedades públicas
        public ItemObject ItemObject => _itemObject;
        public string ItemName => _itemName;
        public Sprite Image => _itemObject.Image;
        public string Description => _description;
        public List<ItemModifier> Modifiers => _modifiers;
        public int Stack => _itemObject.Stack;
        public ItemType ItemType => _itemObject.ItemType;
        public bool IsEquippable => _itemObject.IsEquippable;
        public Mesh Mesh => _itemObject.Mesh;
        public Material[] Materials => _itemObject.Materials;

        public Item()
        {
            _itemObject = null;
            _itemName = string.Empty;
            _description = string.Empty;
            _modifiers = new List<ItemModifier>();
        }
        
        public Item(ItemObject itemObject)
        {
            if (itemObject == null)
            {
                _itemObject = null;
                _itemName = string.Empty;
                _description = string.Empty;
                _modifiers = new List<ItemModifier>();
                return;
            }

            _itemObject = itemObject;
            _itemName = itemObject.ItemName;
            _description = itemObject.Description;
            _modifiers = new List<ItemModifier>();
        }

        public bool IsStackable(Item item)
        {
            return _itemObject == item.ItemObject && _itemName == item.ItemName && _description == item.Description && _modifiers == item.Modifiers;
        }
    }
}