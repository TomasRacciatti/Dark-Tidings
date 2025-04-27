using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [System.Serializable]
    public class Item
    {
        [SerializeField] private ItemObject _itemObject; //for stack, mesh, materials, equipable, type
        [SerializeField] private string _itemName;
        [SerializeField] private string _description;
        [SerializeField] private List<ItemModifier> _modifiers;

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

        public void InitializeItem(ItemObject itemObject)
        {
            if (itemObject == null) return;

            _itemObject = itemObject;
            _itemName = itemObject.ItemName;
            _description = itemObject.Description;
            _modifiers = new List<ItemModifier>();
        }

        public bool IsStackable(Item item)
        {
            return _itemObject == item.ItemObject && _itemName == item.ItemName && _description == item.Description;
        }
    }
}