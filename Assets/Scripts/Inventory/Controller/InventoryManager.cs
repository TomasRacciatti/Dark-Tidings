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
        public GameObject boltUIGameObject;
        
        public Switcher InventorySwitcherUI => inventorySwitcherUI;

        private void Start()
        {
            inventorySwitcherUI.gameObject.SetActive(false);
            DeactiveBoltUI();
        }
        
        public int GetIndexInventory()
        {
            return Switcher.GetIndexSwitcher(inventorySwitcherUI);
        }

        public void ActiveBoltUI()
        {
            CancelInvoke(nameof(DeactiveBoltUI));
            boltUIGameObject.SetActive(true);
            if (GetIndexInventory() == -1)
            {
                Invoke(nameof(DeactiveBoltUI), 5);
            }
        }
        
        public void DeactiveBoltUI()
        {
            boltUIGameObject.SetActive(false);
        }
    }
}