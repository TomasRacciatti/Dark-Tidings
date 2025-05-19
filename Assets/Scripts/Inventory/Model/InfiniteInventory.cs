using System.Collections.Generic;
using UnityEngine;
using Items.Base;

namespace Inventory.Model
{
    public class InfiniteInventory : InventorySystem
    {
        public override int AddItem(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty) return itemAmount.Amount;
            itemAmount.SetAmount(StackItems(itemAmount));
            if (itemAmount.IsEmpty) return itemAmount.Amount;
            AddItemEmptySlot(itemAmount);
            return 0;
        }
        
        public override int RemoveItem(ItemAmount itemAmount)
        {
            return RemoveItemsInternal(itemAmount, i =>
            {
                ClearSlot(i);
                return true;
            });
        }
        
        public override void ClearInventory()
        {
            items = new List<ItemAmount>();
        }
        
        public override void ClearSlot(int i)
        {
            items.RemoveAt(i);
        }
        
        protected override int AddItemEmptySlot(ItemAmount itemAmount)
        {
            while (!itemAmount.IsEmpty)
            {
                ItemAmount newItem = new ItemAmount();
                itemAmount.SetAmount(newItem.SetItem(itemAmount));
                items.Add(newItem);
                
                UpdateItem(items.Count - 1);
            }
            return itemAmount.Amount;
        }
    }
}
