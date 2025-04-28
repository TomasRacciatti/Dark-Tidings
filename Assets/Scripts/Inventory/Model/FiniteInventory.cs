using UnityEngine;
using System.Linq;
using Items;

namespace Inventory.Model
{
    public class FiniteInventory : InventorySystem
    {
        [SerializeField] [Range(4, 50)] private int slotsAmount = 12;

        private void Awake()
        {
            ClearInventory(); //limpia e inicializa los slots, borrar esto despues
        }

        public override int AddItem(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty) return 0;
            itemAmount.RemoveAmount(itemAmount.Amount - StackItems(itemAmount));
            if (itemAmount.IsEmpty) return 0;
            itemAmount.RemoveAmount(itemAmount.Amount - PlaceInEmptySlot(itemAmount));

            return itemAmount.Amount; // amount not added
        }

        public override int RemoveItem(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty) return 0;
            return RemoveItemsInternal(itemAmount, i =>
            {
                items[i] = new ItemAmount(); // mantiene el slot vacío
            });
        }

        public bool IsFull()
        {
            return !items.Exists(item => item.IsEmpty);
        }

        public override void ClearInventory()
        {
            items = Enumerable.Range(0, slotsAmount).Select(_ => new ItemAmount()).ToList();
        }

        public override void ClearSlot(int i)
        {
            items[i] = new ItemAmount();
        }

        public bool SwapItems(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= items.Count || toIndex < 0 || toIndex >= items.Count) return false;
            if (fromIndex == toIndex) return false;

            ItemAmount fromItem = items[fromIndex];
            ItemAmount toItem = items[toIndex];

            if (toItem.IsEmpty || fromItem.IsStackable(toItem))
            {
                int remainingAmount = toItem.SetItem(fromItem);
                items[toIndex] = toItem;
                
                print(remainingAmount);

                if (remainingAmount > 0)
                {
                    fromItem.SetAmount(remainingAmount);
                    items[fromIndex] = fromItem;
                }
                else
                {
                    items[fromIndex] = new ItemAmount();
                }

                UpdateHud(fromIndex);
                UpdateHud(toIndex);
                return remainingAmount <= 0;
            }

            (items[fromIndex], items[toIndex]) = (items[toIndex], items[fromIndex]);
            UpdateHud(fromIndex);
            UpdateHud(toIndex);
            return false;
        }


        private int PlaceInEmptySlot(ItemAmount itemAmount)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.IsEmpty)
                {
                    itemAmount.RemoveAmount(itemAmount.Amount - item.SetItem(itemAmount));
                    items[i] = item;

                    UpdateHud(i);

                    if (itemAmount.Amount <= 0)
                        return 0;
                }
            }

            return itemAmount.Amount;
        }
    }
}