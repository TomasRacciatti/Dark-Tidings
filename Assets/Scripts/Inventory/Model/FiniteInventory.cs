using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Items.Base;

namespace Inventory.Model
{
    public class FiniteInventory : InventorySystem
    {
        [SerializeField] [Range(1, 50)] private int slotsAmount = 12;

        private void Awake()
        {
            if (items == null)
                items = new List<ItemAmount>();
            while (items.Count < slotsAmount)
            {
                items.Add(new ItemAmount());
            }
        }

        public override void AddItem(ref ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty || !IsItemAllowed(itemAmount.SoItem)) return;
            StackItems(ref itemAmount);
            if (itemAmount.IsEmpty) return;
            AddItemEmptySlot(ref itemAmount);
        }

        public override void RemoveItem(ref ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty || !IsItemAllowed(itemAmount.SoItem)) return;

            RemoveItemsInternal(ref itemAmount, i =>
            {
                items[i] = new ItemAmount();
                return false;
            });
        }

        public override void ClearInventory()
        {
            for (int i = 0; i < items.Count; i++)
            {
                ClearSlot(i);
            }
        }

        public override void ClearSlot(int i)
        {
            items[i].Clear();
            NotifyItemChanged(i);
        }
        
        protected override void AddItemEmptySlot(ref ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty || !IsItemAllowed(itemAmount.SoItem)) return;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.IsEmpty)
                {
                    itemAmount.SetAmount(item.SetItem(itemAmount));
                    items[i] = item;

                    NotifyItemChanged(i);
                    if (itemAmount.Amount <= 0)
                        return;
                }
            }
        }
    }
}