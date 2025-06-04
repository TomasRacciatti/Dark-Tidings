using System;
using System.Collections;
using Inventory.Model;
using Inventory.View;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory.Controller
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] public GameObject itemSlotPrefab;
        
        //Slots
        [SerializeField] public ToolbarUI toolbarUI;
        [SerializeField] public InventoryUI inventoryUI;
        [SerializeField] private Switcher inventorySwitcherUI;
        [SerializeField] public InventorySystem boltsInventorySystem; 
        
        public Switcher InventorySwitcherUI => inventorySwitcherUI;

        private void Start()
        {
            inventorySwitcherUI.gameObject.SetActive(false);
        }
        
        public int GetIndexInventory()
        {
            return Switcher.GetIndexSwitcher(inventorySwitcherUI);
        }
    }
}