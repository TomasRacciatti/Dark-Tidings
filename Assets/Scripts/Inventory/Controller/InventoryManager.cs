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

        private Coroutine _coroutine;

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
            CancelInvoke(nameof(DeactiveBoltUI2));
            if (boltPanel.alpha < 1)
            {
                boltUIGameObject.SetActive(true);
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }
                _coroutine = StartCoroutine(Effect.Fade(boltPanel, 0, 1, 0.4f));
            }
            if (GetIndexInventory() == -1)
            {
                Invoke(nameof(DeactiveBoltUI), 5);
            }
        }
        
        public void DeactiveBoltUI()
        {
            if (!boltPanel.gameObject.activeInHierarchy) return;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(Effect.Fade(boltPanel, 1, 0, 0.4f));
            Invoke(nameof(DeactiveBoltUI2), 0.4f);
        }

        private void DeactiveBoltUI2()
        {
            boltUIGameObject.SetActive(false);
        }
    }
}