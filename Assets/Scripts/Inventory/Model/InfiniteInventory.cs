using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Inventory.Model
{
    public class InfiniteInventory : InventorySystem
    {
        public override int AddItem(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty) return itemAmount.Amount;
            itemAmount.RemoveAmount(itemAmount.Amount - StackItems(itemAmount));
            if (itemAmount.IsEmpty) return itemAmount.Amount;
            AddNewItemStacks(itemAmount);
            return 0;
        }
        
        public override int RemoveItem(ItemAmount itemAmount)
        {
            return RemoveItemsInternal(itemAmount, i =>
            {
                ClearSlot(i);
                i--;
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
        
        private void AddNewItemStacks(ItemAmount itemAmount)
        {
            while (!itemAmount.IsEmpty)
            {
                int amountToAdd = Mathf.Min(itemAmount.Amount, itemAmount.Item.Stack);
                itemAmount.RemoveAmount(amountToAdd);
                ItemAmount aaa = new ItemAmount();
                aaa.SetItem(itemAmount);
                items.Add(aaa);

                // Actualiza el HUD del nuevo ítem
                UpdateHud(items.Count - 1);
            }
        }
    }
}
