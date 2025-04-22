using System;
using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] public GameObject itemPrefab;
        
        //Slots
        [SerializeField] public Toolbar toolbar;
        [SerializeField] public InventoryView inventoryView;
        [SerializeField] private InventorySlot selectedSlot;
        
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject backpackUI;
        [SerializeField] private GameObject slotSelector;
        [SerializeField] private GameObject bulletSelector;
        [SerializeField] private GameObject backpack;
        [SerializeField] private GameObject backpackRoot;
        public bool hasBagpack = false;
        
        public static InventoryManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public InventoryItem GetSelectedItem()
        {
            return selectedSlot.GetComponentInChildren<InventoryItem>();
        }

        public bool ToggleInventory()
        {
            bool setActive = !inventoryUI.gameObject.activeSelf;
            inventoryUI.gameObject.SetActive(setActive);
            if (setActive)
            {
                // Mostrar y liberar el cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Ocultar y bloquear el cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            return setActive;
        }
        
        public bool ToggleBackpack()
        {
            hasBagpack = !hasBagpack;
            backpackUI.gameObject.SetActive(hasBagpack);
            return hasBagpack;
        }

        //add item order
        /*
        private IEnumerable<InventorySlot> GetOrder(ItemType itemType)
        {
            List<InventorySlot> slotOrder = new();

            switch (itemType)
            {
                case ItemType.Weapon:
                case ItemType.Tool:
                    slotOrder.AddRange(toolbarSlots);
                    slotOrder.AddRange(inventorySlots);
                    break;

                case ItemType.Armour:
                    slotOrder.AddRange(armorSlots);
                    slotOrder.AddRange(inventorySlots);
                    slotOrder.AddRange(toolbarSlots);
                    break;

                case ItemType.MatMetal:
                    slotOrder.AddRange(inventorySlots);
                    break;

                default:
                    slotOrder.AddRange(toolbarSlots);
                    slotOrder.AddRange(inventorySlots);
                    break;
            }

            if (hasBagpack)
            {
                slotOrder.AddRange(backpackSlots);
            }

            return GetSlotsInCustomOrder(slotOrder.ToArray());
        }

        private IEnumerable<InventorySlot> GetSlotsInCustomOrder(params InventorySlot[][] slotGroups)
        {
            foreach (var group in slotGroups)
            {
                foreach (var slot in group)
                {
                    yield return slot;
                }
            }
        }*/
    }
}