using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InfiniteInventory : InventorySystem
    {
        public override int AddItems(ItemObject itemObject, int amount)
        {
            amount = StackItems(itemObject, amount);
            AddNewItemStacks(itemObject, amount);
            
            return 0;
        }
        
        public override int RemoveItems(ItemObject itemObject, int amount)
        {
            return RemoveItemsInternal(itemObject, amount, i =>
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
        
        private void AddNewItemStacks(ItemObject itemObject, int amount)
        {
            while (amount > 0)
            {
                int amountToAdd = Mathf.Min(amount, itemObject.stack);
                amount -= amountToAdd;
                items.Add(new ItemAmount(itemObject, amountToAdd));
            }
        }
    }
}
