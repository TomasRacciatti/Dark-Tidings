using System.Collections.Generic;
using UnityEngine;
using Items.Base;

namespace Inventory.Model
{
    public class InfiniteInventory : InventorySystem
    {
        public override void AddItem(ref ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty) return;

            StackItems(ref itemAmount);
            if (itemAmount.IsEmpty) return;

            AddItemEmptySlot(ref itemAmount);
        }

        public override void RemoveItem(ref ItemAmount itemAmount)
        {
            RemoveItemsInternal(ref itemAmount, i =>
            {
                ClearSlot(i);
                return true;
            });
        }
        
        public override void ClearInventory()
        {
            items = new List<ItemAmount>();
            NotifyInventoryChanged();
        }
        
        public override void ClearSlot(int i)
        {
            items.RemoveAt(i);
            NotifyItemChanged(i);
        }
        
        protected override void AddItemEmptySlot(ref ItemAmount itemAmount)
        {
            while (!itemAmount.IsEmpty)
            {
                ItemAmount newItem = new ItemAmount();
                itemAmount.SetAmount(newItem.SetItem(itemAmount));
                items.Add(newItem);
                NotifyItemChanged(items.Count - 1);
            }
        }
    }
}
