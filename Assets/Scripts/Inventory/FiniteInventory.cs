using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Items;

namespace Inventory
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