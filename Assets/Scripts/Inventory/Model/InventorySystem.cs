using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory.Controller;
using Items.Base;
using UnityEngine;

namespace Inventory.Model
{
    [Serializable]
    public abstract class InventorySystem : MonoBehaviour
    {
        [SerializeField] protected List<ItemAmount> items = new List<ItemAmount>();
        [SerializeField] public Toolbar toolbar;
        public event Action<int, ItemAmount> OnItemUpdated;
        public event Action OnInventoryUpdated;

        public abstract int AddItem(ItemAmount itemAmount);
        protected abstract int AddItemEmptySlot(ItemAmount itemAmount);
        public abstract int RemoveItem(ItemAmount itemAmount);

        protected void UpdateItemUI(int index)
        {
            OnItemUpdated?.Invoke(index, items[index]);
        }

        protected void UpdateInventoryUI()
        {
            OnInventoryUpdated?.Invoke();
        }

        public ItemAmount GetItem(int slot)
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

        public (int transferred, int remaining) TransferItemTo(InventorySystem otherInventory, int index)
        {
            var item = items[index];
            int remaining = otherInventory.AddItem(item);
            int transferred = item.Amount - remaining;

            item.RemoveAmount(transferred);
            items[index] = item;

            return (transferred, remaining);
        }

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
                    UpdateItemUI(i);

                    if (itemAmount.Amount <= 0)
                        return 0;
                }
            }

            return itemAmount.Amount;
        }

        protected int RemoveItemsInternal(ItemAmount itemAmount, Func<int, bool> onItemEmptied)
        {
            if (itemAmount.IsEmpty) return itemAmount.Amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && item.SoItem == itemAmount.SoItem)
                {
                    itemAmount.SetAmount(item.RemoveAmount(itemAmount.Amount));

                    if (item.IsEmpty)
                        if (onItemEmptied(i)) i--;
                        else items[i] = item;

                    UpdateItemUI(i);
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
                if (toolbar != null) toolbar.SetIndex(fromToolbar, toIndex);

                if (remainingAmount > 0)
                {
                    fromItem.SetAmount(remainingAmount);
                    items[fromIndex] = fromItem;
                    if (toolbar != null) toolbar.SetIndex(toToolbar, fromIndex);
                }
                else
                {
                    ClearSlot(fromIndex);
                    if (toolbar != null) toolbar.SetIndex(toToolbar, -1);
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

        public List<ItemAmount> ConsumeItems(List<ItemAmount> requiredItems)
        {
            List<ItemAmount> missingItems = new List<ItemAmount>();

            foreach (var required in requiredItems)
            {
                int availableAmount = GetItemAmount(required.SoItem);
                int missingAmount = required.Amount - availableAmount;

                if (missingAmount > 0)
                {
                    missingItems.Add(new ItemAmount(required.SoItem, missingAmount));
                }
            }

            return missingItems;
        }

        public static List<ItemAmount> StackItemAmounts(IEnumerable<ItemAmount> items)
        {
            return items
                .Where(item => !item.IsEmpty)
                .GroupBy(item => new { item.SoItem, ModKey = GetModifierKey(item) })
                .Select(group =>
                {
                    var baseItem = group.First();
                    int totalAmount = group.Sum(i => i.Amount);
                    bool hasOverflow = group.Any(i => i.Overflow);
                    var modifiers = baseItem.Modifiers;

                    var stacked = new ItemAmount(baseItem.SoItem, totalAmount, modifiers, hasOverflow);
                    return stacked;
                })
                .ToList();
        }

        private static string GetModifierKey(ItemAmount item)
        {
            if (item.Modifiers == null || item.Modifiers.Count == 0)
                return string.Empty;

            return string.Join(",", item.Modifiers.Select(mod => mod.SoItem.name).OrderBy(name => name));
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
/*
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
        }*/
    }
}