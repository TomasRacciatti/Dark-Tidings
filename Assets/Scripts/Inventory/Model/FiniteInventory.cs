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

        public override int AddItem(ItemObject itemObject, int amount)
        {
            if (itemObject == null && amount <= 0) return 0;
            amount = StackItems(itemObject, amount);
            if (amount <= 0) return amount;
            amount = PlaceInEmptySlot(itemObject, amount);

            return amount; // amount not added
        }

        public override int RemoveItem(ItemObject itemObject, int amount)
        {
            if (itemObject == null && amount <= 0) return 0;
            return RemoveItemsInternal(itemObject, amount, i =>
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

            if (toItem.Item == fromItem.Item)
            {
                int remainingAmount = toItem.SetItem(fromItem.Item, fromItem.Amount + toItem.Amount);
                items[toIndex] = toItem;

                if (remainingAmount > 0)
                {
                    fromItem.SetAmount(remainingAmount);
                    items[fromIndex] = fromItem;
                }
                else
                {
                    items[fromIndex].Clear();
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


        private int PlaceInEmptySlot(ItemObject itemObject, int amount)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.IsEmpty)
                {
                    amount = item.SetItem(itemObject, amount);
                    items[i] = item;

                    UpdateHud(i);

                    if (amount <= 0)
                        return 0;
                }
            }

            return amount;
        }
    }
}