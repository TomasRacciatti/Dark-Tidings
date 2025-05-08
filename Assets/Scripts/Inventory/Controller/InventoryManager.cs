using Inventory.Model;
using Inventory.View;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory.Controller
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] public GameObject itemSlotPrefab;
        
        //Slots
        [SerializeField] public Toolbar toolbar;
        [SerializeField] public InventoryView inventoryView;
        
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject backpackUI;
        
        [SerializeField] public SlotType inventorySlotType;
        [SerializeField] public SlotType toolbarSlotType;

        public void SetActiveInventory(bool active)
        {
            inventoryUI.gameObject.SetActive(active);
        }
        /*
        public void ToggleBackpack(bool active)
        {
            backpackUI.gameObject.SetActive(active);
        }*/
    }
}