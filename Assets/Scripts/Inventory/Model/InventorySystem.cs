using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Controller;
using Items;
using UnityEngine;

namespace Inventory.Model
{
    [System.Serializable]
    public abstract class InventorySystem : MonoBehaviour
    {
        [SerializeField] protected List<ItemAmount> items = new List<ItemAmount>();
        [SerializeField] private Toolbar toolbar;
        
        private void Start()
        {
            UpdateAllHud();
        }

        public abstract int AddItem(ItemAmount itemAmount);
        protected abstract int AddMoreItem(ItemAmount itemAmount);
        public abstract int RemoveItem(ItemAmount itemAmount);

        protected void UpdateHud(int index)
        {
            CanvasManager.Instance.inventoryManager.inventoryView.SetItem(index, items[index]);
        }

        protected void UpdateAllHud()
        {
            for (int i = 0; i < items.Count; i++)
            {
                UpdateHud(i);
            }
        }

        public ItemAmount GetIndexItem(int slot)
        {
            if (slot < 0 || slot >= items.Count) return new ItemAmount();
            
            return items[slot];
        }

        public bool HasItem(SO_Item soItem)
        {
            return HasItemAmount(soItem, 1);
        }

        public bool HasItemAmount(SO_Item soItem, int requiredAmount)
        {
            return GetItemAmount(soItem) > requiredAmount;
        }

        public int GetItemAmount(SO_Item soItem)
        {
            if (soItem == null) return 0;
            int totalAmount = 0;

            foreach (var item in items)
            {
                if (!item.IsEmpty && item.GetSoItem == soItem)
                {
                    totalAmount += item.Amount;
                }
            }

            return totalAmount;
        }

        public abstract void ClearInventory();

        public abstract void ClearSlot(int i);

        public ItemAmount[] GetItemsOfTypes(params Items.ItemType[] types)
        {
            return items.Where(item => !item.IsEmpty && types.Contains(item.GetSoItem.ItemType)).ToArray();
        }

        public void TransferSlotTo(InventorySystem otherInventory, int index)
        {
            var item = items[index];
            otherInventory.AddItem(item);
            ClearSlot(index);
        }

        public void SortItemsByType(ItemType type)
        {
            items = items.Where(item => !item.IsEmpty) // Filtra los ítems vacíos.
                .OrderBy(item => item.GetSoItem.ItemType) // Ordena primero por ItemType
                .ThenBy(item => item.GetSoItem.ItemName) // Luego, ordena por nombre de ítem
                .ToList();
            UpdateAllHud();
        }

        protected int StackItems(ItemAmount itemAmount)
        {
            if (itemAmount.ItemInstance.Stack <= 1) return itemAmount.Amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && itemAmount.IsStackable(item))
                {
                    itemAmount.RemoveAmount(itemAmount.Amount - item.AddAmount(itemAmount.Amount));
                    items[i] = item;
                    UpdateHud(i);

                    if (itemAmount.Amount <= 0)
                        return 0;
                }
            }

            return itemAmount.Amount;
        }

        protected int RemoveItemsInternal(ItemAmount itemAmount, Action<int> onItemEmptied)
        {
            if (itemAmount.ItemInstance == null || itemAmount.Amount <= 0) return itemAmount.Amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && item.IsStackable(itemAmount))
                {
                    itemAmount.RemoveAmount(itemAmount.Amount - item.RemoveAmount(itemAmount.Amount));

                    if (item.IsEmpty)
                    {
                        onItemEmptied(i);
                    }
                    else
                    {
                        items[i] = item;
                    }

                    UpdateHud(i);

                    if (itemAmount.Amount <= 0) return 0;
                }
            }

            return itemAmount.Amount;
        }
        
        public bool SwapItems(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= items.Count || toIndex < 0 || toIndex >= items.Count) return false;
            if (fromIndex == toIndex) return false;

            ItemAmount fromItem = items[fromIndex];
            ItemAmount toItem = items[toIndex];
            
            int fromToolbar = toolbar.GetIndex(fromIndex);
            int toToolbar = toolbar.GetIndex(toIndex);

            if (toItem.IsEmpty || fromItem.IsStackable(toItem))
            {
                int remainingAmount = toItem.SetItem(fromItem);
                items[toIndex] = toItem;
                if (toolbar != null)
                {
                    toolbar.SetIndex(fromToolbar, toIndex);
                }
                
                if (remainingAmount > 0)
                {
                    fromItem.SetAmount(remainingAmount);
                    items[fromIndex] = fromItem;
                    if (toolbar != null)
                    {
                        toolbar.SetIndex(toToolbar, fromIndex);
                    }
                }
                else
                {
                    ClearSlot(fromIndex);
                    if (toolbar != null)
                    {
                        toolbar.SetIndex(toToolbar, -1);
                    }
                }
                return remainingAmount <= 0;
            }
            
            (items[fromIndex], items[toIndex]) = (items[toIndex], items[fromIndex]);
            if (toolbar != null)
            {
                toolbar.SetIndex(fromToolbar, toIndex);
                toolbar.SetIndex(toToolbar, fromIndex);
            }
            
            return false;
        }
    }
}