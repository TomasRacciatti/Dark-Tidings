using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.View;
using Items;
using UnityEngine;

namespace Inventory.Model
{
    [System.Serializable]
    public abstract class InventorySystem : MonoBehaviour
    {
        [SerializeField] protected List<ItemAmount> items = new List<ItemAmount>();
        
        private void Start()
        {
            UpdateAllHud();
        }

        public abstract int AddItem(ItemAmount itemAmount);
        public abstract int RemoveItem(ItemAmount itemAmount);

        protected void UpdateHud(int index)
        {
            CanvasGameManager.Instance.inventoryManager.inventoryView.SetItem(index, items[index]);
        }

        protected void UpdateAllHud()
        {
            for (int i = 0; i < items.Count; i++)
            {
                UpdateHud(i);
            }
        }

        public bool HasItem(ItemObject itemObject)
        {
            return HasItemAmount(itemObject, 1);
        }

        public bool HasItemAmount(ItemObject itemObject, int requiredAmount)
        {
            return GetItemAmount(itemObject) > requiredAmount;
        }

        public int GetItemAmount(ItemObject itemObject)
        {
            if (itemObject == null) return 0;
            int totalAmount = 0;

            foreach (var item in items)
            {
                if (!item.IsEmpty && item.GetItemObject == itemObject)
                {
                    totalAmount += item.Amount;
                }
            }

            return totalAmount;
        }

        public abstract void ClearInventory();

        public abstract void ClearSlot(int i);

        public ItemAmount[] GetItemsOfTypes(params Items.ItemType[] types)
        {
            return items.Where(item => !item.IsEmpty && types.Contains(item.GetItemObject.ItemType)).ToArray();
        }

        public void TransferSlotTo(InventorySystem otherInventory, int index)
        {
            var item = items[index];
            otherInventory.AddItem(item);
            ClearSlot(index);
        }

        public void SortItemsByType(ItemType type)
        {
            items = items.Where(item => !item.IsEmpty) // Filtra los ítems vacíos.
                .OrderBy(item => item.GetItemObject.ItemType) // Ordena primero por ItemType
                .ThenBy(item => item.GetItemObject.ItemName) // Luego, ordena por nombre de ítem
                .ToList();
            UpdateAllHud();
        }

        protected int StackItems(ItemAmount itemAmount)
        {
            if (itemAmount.ItemInstance.Stack <= 1) return itemAmount.Amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && itemAmount.IsStackable(item))
                {
                    itemAmount.RemoveAmount(itemAmount.Amount - item.AddAmount(itemAmount.Amount));
                    items[i] = item;
                    UpdateHud(i);

                    if (itemAmount.Amount <= 0)
                        return 0;
                }
            }

            return itemAmount.Amount;
        }

        protected int RemoveItemsInternal(ItemAmount itemAmount, Action<int> onItemEmptied)
        {
            if (itemAmount.ItemInstance == null || itemAmount.Amount <= 0) return itemAmount.Amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.IsEmpty && item.IsStackable(itemAmount))
                {
                    itemAmount.RemoveAmount(itemAmount.Amount - item.RemoveAmount(itemAmount.Amount));

                    if (item.IsEmpty)
                    {
                        onItemEmptied(i);
                    }
                    else
                    {
                        items[i] = item;
                    }

                    UpdateHud(i);

                    if (itemAmount.Amount <= 0) return 0;
                }
            }

            return itemAmount.Amount;
        }
    }
}