using System.Collections;
using Inventory.Model;
using Inventory.View;
using Managers;
using Patterns;
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
        
        [SerializeField] public SlotType inventorySlotType;
        [SerializeField] public SlotType toolbarSlotType;
        [SerializeField] public SlotType ammoSlotType;

        [SerializeField] private Switcher inventorySwitcherUI;
        public Switcher InventorySwitcherUI => inventorySwitcherUI;
        
        private void Start()
        {
            StartCoroutine(ExecuteNextFrame());
        }
    
        private IEnumerator ExecuteNextFrame()
        {
            yield return null;
            SetInventory();
        }

        private void SetInventory()
        {
            inventoryUI.SetInventory(GameManager.Player.inventory);
        }
        
        public int GetIndexInventory()
        {
            return Switcher.GetIndexSwitcher(inventorySwitcherUI);
        }
    }
}