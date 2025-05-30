using System.Collections.Generic;
using System.Linq;
using Inventory.Interfaces;
using Items.Base;

namespace Inventory.Model
{
    public static class InventoryUtility
    {
        public static InventorySystem SetInventoryObserver(InventorySystem oldInventory, InventorySystem newInventory, IInventoryObserver observer)
        {
            if (oldInventory != null)
            {
                oldInventory.RemoveObserver(observer);
            }

            if (newInventory != null)
            {
                newInventory.AddObserver(observer);
                observer.OnInventoryChanged(newInventory.items);
            }
            
            return newInventory;
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
    }
}