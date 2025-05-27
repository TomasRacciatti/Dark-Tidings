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
            InitializeSlots(); //limpia e inicializa los slots, borrar esto despues
        }
        
        private void InitializeSlots()
        {
            items = Enumerable.Range(0, slotsAmount)
                .Select(_ => new ItemAmount())
                .ToList();
            UpdateInventoryUI();
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

        public bool IsFull()
        {
            return !items.Exists(item => item.IsEmpty);
        }

        public override void ClearInventory()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i] = new ItemAmount();
                //UpdateItem(i);
            }
            UpdateInventoryUI();
        }

        public override void ClearSlot(int i)
        {
            items[i].Clear();
        }
        
        protected override int AddItemEmptySlot(ItemAmount itemAmount)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.IsEmpty)
                {
                    itemAmount.SetAmount(item.SetItem(itemAmount));
                    items[i] = item;

                    print("2");
                    UpdateItemUI(i);

                    if (itemAmount.Amount <= 0)
                        return 0;
                }
            }

            return itemAmount.Amount;
        }
    }
}