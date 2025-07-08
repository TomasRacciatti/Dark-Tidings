using System;
using System.Collections;
using Inventory.Model;
using Inventory.View;
using Managers;
using UnityEngine;
using Effects;
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
        [SerializeField] private CanvasGroup boltPanel;
        
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
            if (boltPanel.gameObject.activeInHierarchy) return;
            CancelInvoke(nameof(DeactiveBoltUI));
            boltUIGameObject.SetActive(true);
            StartCoroutine(Effect.Fade(boltPanel ,0, 1, 0.4f));
            if (GetIndexInventory() == -1)
            {
                Invoke(nameof(DeactiveBoltUI), 5);
            }
        }
        
        public void DeactiveBoltUI()
        {
            if (!boltPanel.gameObject.activeInHierarchy) return;
            StartCoroutine(Effect.Fade(boltPanel ,1, 0, 0.4f));
            boltUIGameObject.SetActive(false);
        }
    }
}