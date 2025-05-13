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

        private void Start()
        {
            ItemEquippable[] equippables = GetComponentsInChildren<ItemEquippable>(includeInactive: true);

            // Si quieres solo los hijos (y no el objeto actual), puedes filtrarlos:
            items = new List<ItemEquippable>();
            foreach (var item in equippables)
            {
                if (item.gameObject != this.gameObject)
                    items.Add(item);
            }
        }

        public void Use()
        {
            if (selectedItem != null)
            {
                selectedItem.Use();
            }
        }

        public void SetItemEquipped(SO_Item soItem = null)
        {
            if (selectedItem != null)
            {
                selectedItem.gameObject.SetActive(false);
            }

            foreach (var item in items)
            {
                bool shouldBeActive = item.soItem == soItem;
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
