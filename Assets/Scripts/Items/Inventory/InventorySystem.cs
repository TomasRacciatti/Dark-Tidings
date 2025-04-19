using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    [System.Serializable]
    public abstract class InventorySystem : MonoBehaviour
    {
        [SerializeField] protected List<ItemAmount> items = new List<ItemAmount>();

        public abstract int AddItem(ItemObject itemObject, int amount);
        public abstract int RemoveItem(ItemObject itemObject, int amount);

        public int GetItemAmount(ItemObject itemObject)
        {
            if (itemObject == null) return 0;
            int totalAmount = 0;

            foreach (var item in items)
            {
                if (!item.IsEmpty && item.item == itemObject)
                {
                    totalAmount += item.amount;
                }
            }

            return totalAmount;
        }


        public bool HasItemAmount(ItemObject itemObject, int requiredAmount) //0 if has enough
        {
            if (itemObject == null) return false;
            if (requiredAmount <= 0) return true;

            int total = 0;

            foreach (var item in items)
            {
                if (!item.IsEmpty && item.item == itemObject)
                {
                    total += item.amount;
                    if (total >= requiredAmount) return true;
                }
            }

            return false; //not enough
        }

        public abstract void ClearInventory();

        public abstract void ClearSlot(int i);

        public ItemAmount[] GetItemsOfTypes(params ItemType[] types)
        {
            return items.Where(item => !item.IsEmpty && types.Contains(item.item.type)).ToArray();
        }
        
        public void SwapItems(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= items.Count || toIndex < 0 || toIndex >= items.Count)
            {
                Debug.LogWarning("Índice fuera de los límites.");
                return; // Salir si los índices no son válidos.
            }
            
            (items[fromIndex], items[toIndex]) = (items[toIndex], items[fromIndex]);
        }

        public void TransferSlotTo(InventorySystem otherInventory, int index)
        {
            var item = items[index];
            otherInventory.AddItem(item.item, item.amount);
            ClearSlot(index);
        }
        
        protected int StackItems(ItemObject itemObject, int amount)
        {
            if (itemObject.stack > 1)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];

                    if (item.CanStackWith(itemObject))
                    {
                        amount = item.AddAmount(amount);
                        items[i] = item;

                        if (amount <= 0)
                            return 0;
                    }
                }
            }

            return amount;
        }
        
        protected int RemoveItemsInternal(ItemObject itemObject, int amount, Action<int> onItemEmptied)
        {
            if (itemObject == null || amount <= 0) return amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && item.item == itemObject)
                {
                    amount = item.RemoveAmount(amount);

                    if (item.IsEmpty)
                    {
                        onItemEmptied(i);
                    }
                    else
                    {
                        items[i] = item;
                    }

                    if (amount <= 0) return 0;
                }
            }

            return amount;
        }
    }
}