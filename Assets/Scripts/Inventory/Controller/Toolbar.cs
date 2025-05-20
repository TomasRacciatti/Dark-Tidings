using System;
using Inventory.Model;
using Items.Base;
using Managers;
using UnityEngine;

namespace Inventory.Controller
{
    public class Toolbar : MonoBehaviour
    {
        [SerializeField] private int selectedSlot = 0;
        [SerializeField] private int[] inventoryIndexes;
        [SerializeField] private int slotCount = 4;
        [SerializeField] private InventorySystem inventorySystem;
        
        public event Action<int> OnSelectedSlotChanged;
        
        public InventorySystem InventorySystem => inventorySystem;

        public int GetSelectedSlot => inventoryIndexes[selectedSlot];
        public int GetSlotIndex(int slot) => inventoryIndexes[slot];

        public ItemAmount GetSlotItem()
        {
            if (GetSelectedSlot == -1) return new ItemAmount();
            return inventorySystem.GetIndexItem(GetSelectedSlot);
        }

        private void Awake()
        {
            if (inventorySystem == null)
            {
                inventorySystem = GetComponentInParent<InventorySystem>();
            }
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            inventoryIndexes = new int[slotCount];
            ClearSlots();
        }

        private void ClearSlots()
        {
            Array.Fill(inventoryIndexes, -1);
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
            print(toolbarSlotIndex + " : " + inventoryIndex);
            
            if (toolbarSlotIndex < 0 || toolbarSlotIndex >= inventoryIndexes.Length) return;
            
            if (inventoryIndex != -1)
            {
                ItemAmount item = inventorySystem.GetIndexItem(inventoryIndex);
                if (!item.ItemInstance.SoItem.IsEquippable) return;
                for (int i = 0; i < inventoryIndexes.Length; i++)
                {
                    if (i != toolbarSlotIndex && inventoryIndexes[i] == inventoryIndex)
                    {
                        inventoryIndexes[i] = -1;
                        SetUI();
                        break;
                    }
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
            GameManager.Canvas.inventoryManager.toolbarView.ChangeSelectedSlot(selectedSlot);
        }

        public void SwapIndexes(int fromSlot, int toSlot)
        {
            (inventoryIndexes[fromSlot], inventoryIndexes[toSlot]) = (inventoryIndexes[toSlot], inventoryIndexes[fromSlot]);
        }
    }
}