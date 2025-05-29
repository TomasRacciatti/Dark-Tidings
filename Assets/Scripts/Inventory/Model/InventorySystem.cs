using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory.Controller;
using Inventory.Interfaces;
using Items.Base;
using UnityEngine;

namespace Inventory.Model
{
    [Serializable]
    public abstract class InventorySystem : MonoBehaviour
    {
        [SerializeField] public List<ItemAmount> items = new();

        private List<IInventoryObserver> _observers = new();

        public abstract int AddItem(ItemAmount itemAmount);
        protected abstract int AddItemEmptySlot(ItemAmount itemAmount);
        public abstract int RemoveItem(ItemAmount itemAmount);
        public abstract void ClearInventory();
        public abstract void ClearSlot(int i);
        
        public void SetItemByIndex(int slot, ItemAmount itemAmount)
        {
            if (slot < 0 || slot >= items.Count) return;
            if (itemAmount.IsEmpty)
            {
                ClearSlot(slot);
                return;
            }

            items[slot] = itemAmount;
            NotifyItemChanged(slot);
        }

        //observer
        public void AddObserver(IInventoryObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void RemoveObserver(IInventoryObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyItemChanged(int index)
        {
            foreach (var observer in _observers)
                observer.OnItemChanged(index, items[index]);
        }

        protected void NotifyInventoryChanged()
        {
            foreach (var observer in _observers)
                observer.OnInventoryChanged(items);
        }

        public ItemAmount GetItemByIndex(int slot)
        {
            if (slot < 0 || slot >= items.Count) return new ItemAmount();

            return items[slot];
        }
        
        public ItemAmount GetFirstSoItem(SO_Item soItem)
        {
            foreach (var item in items)
            {
                if (item.SoItem == soItem) return new ItemAmount(item);
            }

            return new ItemAmount();
        }

        public bool HasItem(SO_Item soItem)
        {
            return HasAmountBySoItem(soItem, 1);
        }

        public bool HasAmountBySoItem(SO_Item soItem, int requiredAmount)
        {
            return GetAmountBySoItem(soItem) >= requiredAmount;
        }

        public int GetAmountBySoItem(SO_Item soItem)
        {
            if (!soItem) return 0;

            return items
                .Where(item => !item.IsEmpty && item.SoItem == soItem)
                .Sum(item => item.Amount);
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

        protected int StackItems(ItemAmount itemAmount)
        {
            if (itemAmount.SoItem.Stack <= 1) return itemAmount.Amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && ItemsUtility.AreStackable(itemAmount, item))
                {
                    itemAmount.SetAmount(item.AddAmount(itemAmount.Amount));
                    items[i] = item;
                    NotifyItemChanged(i);

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

                    NotifyItemChanged(i);
                    if (itemAmount.Amount <= 0) return 0;
                }
            }

            return itemAmount.Amount;
        }

        public void TransferIndexToIndex(InventorySystem targetInventory, int fromIndex, int targetIndex)
        {
            if (fromIndex < 0 || fromIndex >= items.Count) return;
            if (targetInventory == null) return;
            if (targetIndex < 0 || targetIndex >= targetInventory.items.Count) return;
            if (this == targetInventory && fromIndex == targetIndex) return;

            ItemAmount fromItem = items[fromIndex];
            if (fromItem.IsEmpty) return;

            ItemAmount targetItem = targetInventory.GetItemByIndex(targetIndex);
            
            if (targetItem.IsEmpty) //si esta vacio
            {
                targetInventory.SetItemByIndex(targetIndex, new ItemAmount(fromItem));
                ClearSlot(fromIndex);
                NotifyItemChanged(fromIndex);
                return;
            }
            
            if (ItemsUtility.AreStackable(fromItem, targetItem)) //si son stackeables
            {
                int remaining = targetInventory.items[targetIndex].AddAmount(fromItem.Amount);
                targetInventory.NotifyItemChanged(targetIndex);

                items[fromIndex].SetAmount(remaining);
                NotifyItemChanged(fromIndex);
                return;
            }

            // Si no son stackeables, hacemos un swap
            targetInventory.SetItemByIndex(targetIndex, new ItemAmount(fromItem));
            SetItemByIndex(fromIndex, targetItem);
        }


        public List<ItemAmount> ConsumeItems(List<ItemAmount> requiredItems)
        {
            List<ItemAmount> missingItems = new List<ItemAmount>();

            foreach (var required in requiredItems)
            {
                int availableAmount = GetAmountBySoItem(required.SoItem);
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

                    var stacked = new ItemAmount(baseItem.SoItem, totalAmount, modifiers, true);
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