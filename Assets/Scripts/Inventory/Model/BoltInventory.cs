using System.Collections.Generic;
using Items.Base;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Model
{
    public class BoltInventory : FiniteInventory
    {
        [SerializeField] private SO_Item SO_bullet;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image image;
        [SerializeField] private List<SO_Item> metals;
        private ItemAmount _itemCraft;
        private InventorySystem _playerInventorySystem;

        private void Start()
        {
            _playerInventorySystem = GameManager.Player.inventory;
        }

        protected override void NotifyItemChanged(int index)
        {
            base.NotifyItemChanged(index);
            UpdateCrafting();
        }
        
        private void UpdateCrafting()
        {
            var item0 = Items[0];
            var item1 = Items[1];
            var item2 = Items[2];

            // Condición base: ambos ítems existen y son iguales
            bool baseValid = !item0.IsEmpty && !item1.IsEmpty && item0.SoItem == item1.SoItem && metals.Contains(item0.SoItem);

            // Condición modificador válido: item2 es null o distinto del base
            bool modifierValid = item2.IsEmpty || (item2.SoItem != item0.SoItem && metals.Contains(item2.SoItem));

            if (baseValid && modifierValid)
            {
                var bullet = new ItemAmount(SO_bullet, 5);
                bullet.AddModifier(new ItemAmount(!item2.IsEmpty ? item2.SoItem : item0.SoItem, 1));
                _itemCraft = bullet;
            }
            else
            {
                _itemCraft = new ItemAmount(null, 0);
            }
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            if (!_itemCraft.IsEmpty)
            {
                text.text = "Craft " + _itemCraft.ItemName;
                image.sprite = _itemCraft.SoItem.Image;
                return;
            }
            text.text = "No Recipe";
            image.sprite = null;
        }

        public void CraftItem()
        {
            if (_itemCraft.IsEmpty) return;
            if (!Items[3].IsEmpty) return;
            SetItemByIndex(3, _itemCraft);
            RemoveItemByIndex(0,1);
            RemoveItemByIndex(1,1);
            RemoveItemByIndex(2,1);
            UpdateCrafting();
        }

        public void GiveItemCrafted()
        {
            if (Items[3].IsEmpty) return;
            var craftedItem = Items[3];
            _playerInventorySystem.AddItem(ref craftedItem);
            Items[3] = craftedItem;
            //Items[3].Clear();
            NotifyItemChanged(3);
        }

        private void OnDisable()
        {
            ClearCraft();
        }

        public void ClearCraft()
        {
            var dropItems = _playerInventorySystem.AddItems(Items);
            foreach (var item in dropItems)
            {
                ItemDropper.Drop(item);
            }
            ClearInventory();
        }
    }
}