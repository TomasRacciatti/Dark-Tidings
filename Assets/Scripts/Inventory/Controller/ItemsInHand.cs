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
        public static ItemsInHand Instance;

        private List<ItemEquippable> _items;

        public ItemEquippable selectedItem;

        private void Awake()
        {
            Instance = this;
            ItemEquippable[] equippables = GetComponentsInChildren<ItemEquippable>(includeInactive: true);
            
            _items = new List<ItemEquippable>();
            foreach (var item in equippables)
            {
                _items.Add(item);
            }
        }

        public static void Use(UseType useType = UseType.Default)
        {
            if (Instance.selectedItem != null)
            {
                Instance.selectedItem.Use(useType);
            }
        }

        public void SetItemEquipped(SO_Item soItem = null)
        {
            if (Instance.selectedItem != null)
            {
                Instance.selectedItem.gameObject.SetActive(false);
            }

            foreach (var item in Instance._items)
            {
                bool shouldBeActive = item.soItem == soItem;
                item.gameObject.SetActive(shouldBeActive);

                if (shouldBeActive)
                {
                    Instance.selectedItem = item;
                    return;
                }
            }

            Instance.selectedItem = null;
        }
    }
}
