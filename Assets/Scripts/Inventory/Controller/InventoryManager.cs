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
        [SerializeField] public ToolbarView toolbarView;
        [SerializeField] public InventoryView inventoryView;
        
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject backpackUI;
        
        [SerializeField] public SlotType inventorySlotType;
        [SerializeField] public SlotType toolbarSlotType;
        
        private void Start()
        {
            inventoryView.SetInventory(GameManager.Player.inventory);
        }

        public void SetActiveInventory(bool active)
        {
            inventoryUI.gameObject.SetActive(active);
        }
    }
}