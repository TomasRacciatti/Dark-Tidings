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
            itemAmount.RemoveAmount(itemAmount.Amount - AddMoreItem(itemAmount));

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
            items = Enumerable.Range(0, slotsAmount).Select(_ => new ItemAmount(null, 0)).ToList();
        }

        public override void ClearSlot(int i)
        {
            items[i] = new ItemAmount();
        }
        
        protected override int AddMoreItem(ItemAmount itemAmount)
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