using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Inventory.Controller;
using Items.Base;
using UnityEngine;

namespace Inventory.Model
{
    [System.Serializable]
    public abstract class InventorySystem : MonoBehaviour
    {
        [SerializeField] protected List<ItemAmount> items = new List<ItemAmount>();
        [SerializeField] private Toolbar toolbar;
        public event Action<int, ItemAmount> OnItemUpdated;
        public event Action OnInventoryUpdated;

        public abstract int AddItem(ItemAmount itemAmount);
        protected abstract int AddItemEmptySlot(ItemAmount itemAmount);
        public abstract int RemoveItem(ItemAmount itemAmount);

        protected void UpdateItem(int index)
        {
            OnItemUpdated?.Invoke(index, items[index]);
        }
        
        protected void UpdateInventory()
        {
            OnInventoryUpdated?.Invoke();
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
            return GetItemAmount(soItem) >= requiredAmount;
        }

        public int GetItemAmount(SO_Item soItem)
        {
            if (soItem == null) return 0;

            return items
                .Where(item => !item.IsEmpty && item.SoItem == soItem)
                .Sum(item => item.Amount);
        }

        public abstract void ClearInventory();

        public abstract void ClearSlot(int i);

        public ItemAmount[] GetItemsOfTypes(params ItemType[] types)
        {
            return items.Where(item => !item.IsEmpty && types.Contains(item.SoItem.ItemType)).ToArray();
        }

        public Dictionary<SO_Item, int> GetAmountByItem()
        {
            return items
                .Where(item => !item.IsEmpty)
                .Aggregate(
                    new Dictionary<SO_Item, int>(),
                    (acc, item) =>
                    {
                        var soItem = item.SoItem;
                        acc.TryAdd(soItem, 0);

                        acc[soItem] += item.Amount;
                        return acc;
                    });
        }

        public (int transferred, int remaining) TransferItemTo(InventorySystem otherInventory, int index)
        {
            var item = items[index];
            int remaining = otherInventory.AddItem(item);
            int transferred = item.Amount - remaining;

            item.RemoveAmount(transferred);
            items[index] = item;

            return (transferred, remaining);
        }
/*
        public void SortItemsByType(ItemType type)
        {
            items = items.Where(item => !item.IsEmpty)
                .OrderBy(item => item.GetSoItem.ItemType)
                .ThenBy(item => item.GetSoItem.ItemName)
                .ToList();
            //asegurarme que sea del mismo size
            //actualizar hud
        }*/

        protected int StackItems(ItemAmount itemAmount)
        {
            if (itemAmount.SoItem.Stack <= 1) return itemAmount.Amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && itemAmount.IsStackable(item))
                {
                    itemAmount.SetAmount(item.AddAmount(itemAmount.Amount));
                    items[i] = item;
                    UpdateItem(i);

                    if (itemAmount.Amount <= 0)
                        return 0;
                }
            }

            return itemAmount.Amount;
        }

        protected int RemoveItemsInternal(ItemAmount itemAmount, Func<int, bool> onItemEmptied)
        {
            if (itemAmount == null || itemAmount.Amount <= 0) return itemAmount.Amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && item.IsStackable(itemAmount))
                {
                    itemAmount.SetAmount(item.RemoveAmount(itemAmount.Amount));

                    if (item.IsEmpty)
                    {
                        bool removed = onItemEmptied(i);
                        if (removed)
                        {
                            i--;
                        }
                    }
                    else
                    {
                        items[i] = item;
                    }

                    UpdateItem(i);
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
/*
        public IEnumerable<ItemAmount> GetItemsByType(ItemType type)
        {
            foreach (var item in items)
            {
                if (!item.IsEmpty && item.GetSoItem.ItemType == type)
                    yield return item;
            }
        }

        public bool TryCraft(Dictionary<SO_Item, int> requiredItems, ItemAmount itemCrafted)
        {
            foreach (var requirement in requiredItems)
            {
                if (!HasItemAmount(requirement.Key, requirement.Value))
                {
                    return false;
                }
            }

            foreach (var requirement in requiredItems)
            {
                int amountToRemove = requirement.Value;

                for (int i = 0; i < items.Count && amountToRemove > 0; i++)
                {
                    var item = items[i];
                    if (!item.IsEmpty && item.GetSoItem == requirement.Key)
                    {
                        int removed = item.RemoveAmount(amountToRemove);
                        amountToRemove -= removed;

                        if (item.IsEmpty)
                        {
                            ClearSlot(i);
                        }
                        else
                        {
                            items[i] = item;
                            UpdateItem(i);
                        }
                    }
                }
            }

            return true;
        }*/
    }
}