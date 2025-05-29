using UnityEngine;
using System.Linq;
using Items.Base;

namespace Inventory.Model
{
    public class FiniteInventory : InventorySystem
    {
        [SerializeField] [Range(4, 50)] private int slotsAmount = 12;

        private void Awake()
        {
            items = Enumerable.Range(0, slotsAmount)
                .Select(_ => new ItemAmount())
                .ToList();
            NotifyInventoryChanged();
        }
        
        public override int AddItem(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty) return 0;
            itemAmount.SetAmount(StackItems(itemAmount));
            if (itemAmount.IsEmpty) return 0;
            itemAmount.SetAmount(AddItemEmptySlot(itemAmount));

            return itemAmount.Amount; // amount not added
        }

        public override int RemoveItem(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty) return 0;
            return RemoveItemsInternal(itemAmount, i =>
            {
                items[i] = new ItemAmount();
                return false;
            });
        }

        public override void ClearInventory()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items.Clear();
                //NotifyItemChanged(i);
            }
            NotifyInventoryChanged();
        }

        public override void ClearSlot(int i)
        {
            items[i].Clear();
            NotifyItemChanged(i);
        }
        
        protected override int AddItemEmptySlot(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty) return itemAmount.Amount;
            
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.IsEmpty)
                {
                    itemAmount.SetAmount(item.SetItem(itemAmount));
                    items[i] = item;
                    
                    NotifyItemChanged(i);

                    if (itemAmount.Amount <= 0)
                        return 0;
                }
            }

            return itemAmount.Amount;
        }
    }
}