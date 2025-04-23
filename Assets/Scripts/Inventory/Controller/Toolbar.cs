using Inventory.View;
using UnityEngine;

namespace Inventory.Controller
{
    public class Toolbar : MonoBehaviour
    {
        [SerializeField] private InventorySlot[] toolbarSlots;
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private GameObject slotSelector;
        
        public InventorySlot SelectedSlot
        {
            get
            {
                if (toolbarSlots == null || toolbarSlots.Length == 0) return null;
                if (selectedSlot < 0 || selectedSlot >= toolbarSlots.Length) return null;
                return toolbarSlots[selectedSlot];
            }
        }
        
        private void Start()
        {
            ChangeSelectedSlot(0);
        }
        
        public void ChangeSelectedSlot(int slot)
        {
            if (selectedSlot == slot) return;
            if (slot >= toolbarSlots.Length) return;

            selectedSlot = slot;
            slotSelector.transform.SetParent(SelectedSlot.transform, false);
            slotSelector.transform.localPosition = Vector3.zero;
        }
        
        public InventoryItem GetSelectedItem()
        {
            return SelectedSlot.GetComponentInChildren<InventoryItem>();
        }

        public void EquipItem(ItemAmount itemAmount, int slot)
        {
            if (slot >= toolbarSlots.Length) return;
            
            InventoryItem inventoryItem = toolbarSlots[slot].GetComponentInChildren<InventoryItem>();
            if (!inventoryItem)
            {
                GameObject newItem = Instantiate(InventoryManager.Instance.itemPrefab, toolbarSlots[slot].transform);
                inventoryItem = newItem.GetComponent<InventoryItem>();
            }
            inventoryItem.SetItem(itemAmount.item, itemAmount.amount);
        }
    }
}