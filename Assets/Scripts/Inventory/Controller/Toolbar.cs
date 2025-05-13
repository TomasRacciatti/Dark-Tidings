using Inventory.Model;
using UnityEngine;

namespace Inventory.Controller
{
    public class Toolbar : MonoBehaviour
    {
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private int[] inventoryIndexes;
        [SerializeField] private int slotCount = 4;

        private InventorySystem inventorySystem;

        public int GetSelectedSlot => inventoryIndexes[selectedSlot];
        public int GetSlotIndex(int slot) => inventoryIndexes[slot];

        private void Awake()
        {
            inventorySystem = GetComponentInParent<InventorySystem>();
            ClearSlots();
        }

        private void ClearSlots()
        {
            inventoryIndexes = new int[slotCount];
            for (int i = 0; i < inventoryIndexes.Length; i++)
            {
                inventoryIndexes[i] = -1;
            }
        }

        public int GetIndex(int inventoryIndex)
        {
            for (int i = 0; i < inventoryIndexes.Length; i++)
            {
                if (inventoryIndex == inventoryIndexes[i])
                {
                    return i;
                }
            }
            return -1;
        }
        
        public void SetIndex(int toolbarSlotIndex, int inventoryIndex)
        {
            if (toolbarSlotIndex < 0 || toolbarSlotIndex >= inventoryIndexes.Length) return;

            if (inventoryIndex == -1)
            {
                inventoryIndexes[toolbarSlotIndex] = inventoryIndex;
                SetUI();
                return;
            }
            
            for (int i = 0; i < inventoryIndexes.Length; i++)
            {
                if (i != toolbarSlotIndex && inventoryIndexes[i] == inventoryIndex)
                {
                    inventoryIndexes[i] = -1;
                    SetUI();
                    break;
                }
            }

            inventoryIndexes[toolbarSlotIndex] = inventoryIndex;
            SetUI();
        }

        private void SetUI()
        {
            //todo logica de ui
        }

        public void SetSelectedSlot(int toolbarSlotIndex)
        {
            if (toolbarSlotIndex < 0 || toolbarSlotIndex >= inventoryIndexes.Length) return;
            
            selectedSlot = toolbarSlotIndex;
        }

        public void SwapIndexes(int fromSlot, int toSlot)
        {
            (inventoryIndexes[fromSlot], inventoryIndexes[toSlot]) = (inventoryIndexes[toSlot], inventoryIndexes[fromSlot]);
        }
    }
}