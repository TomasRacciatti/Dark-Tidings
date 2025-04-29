using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory.Controller
{
    public class ItemsInHand : MonoBehaviour
    {
        public static ItemsInHand Instance { get; private set; }

        [SerializeField] private List<ItemEquippable> items;

        public ItemEquippable selectedItem;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Use()
        {
            if (selectedItem != null)
            {
                selectedItem.Interact();
            }
        }

        public void SetItemEquipped(ItemObject itemObject = null)
        {
            if (selectedItem != null)
            {
                selectedItem.gameObject.SetActive(false);
            }

            foreach (var item in items)
            {
                bool shouldBeActive = item.itemObject == itemObject;
                item.gameObject.SetActive(shouldBeActive);

                if (shouldBeActive)
                {
                    selectedItem = item;
                    return;
                }
            }

            selectedItem = null;
        }
    }
}
