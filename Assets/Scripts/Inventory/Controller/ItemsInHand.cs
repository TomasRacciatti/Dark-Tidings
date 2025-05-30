using System;
using System.Collections.Generic;
using Items;
using Items.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory.Controller
{
    public class ItemsInHand : MonoBehaviour
    {
        private static ItemsInHand _instance;

        private List<ItemEquippable> _items;

        public ItemEquippable selectedItem;

        private void Awake()
        {
            _instance = this;
            ItemEquippable[] equippables = GetComponentsInChildren<ItemEquippable>(includeInactive: true);
            
            _items = new List<ItemEquippable>();
            foreach (var item in equippables)
            {
                _items.Add(item);
            }
        }

        public static void Use()
        {
            if (_instance.selectedItem != null)
            {
                _instance.selectedItem.Use();
            }
        }

        public static void SetItemEquipped(SO_Item soItem = null)
        {
            if (_instance.selectedItem != null)
            {
                _instance.selectedItem.gameObject.SetActive(false);
            }

            foreach (var item in _instance._items)
            {
                bool shouldBeActive = item.soItem == soItem;
                item.gameObject.SetActive(shouldBeActive);

                if (shouldBeActive)
                {
                    _instance.selectedItem = item;
                    return;
                }
            }

            _instance.selectedItem = null;
        }
    }
}
