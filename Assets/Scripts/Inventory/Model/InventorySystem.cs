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
        [SerializeField] public InventoryView _view;
        
        private void Start()
        {
            _view = CanvasGameManager.Instance.inventoryManager.inventoryView; //cambiar esto todo
            UpdateAllHud();
        }

        public abstract int AddItem(ItemObject itemObject, int amount);
        public abstract int RemoveItem(ItemObject itemObject, int amount);

        protected void UpdateHud(int index)
        {
            _view.SetItem(index, items[index]);
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
                if (!item.IsEmpty && item.Item == itemObject)
                {
                    totalAmount += item.Amount;
                }
            }

            return totalAmount;
        }

        public abstract void ClearInventory();

        public abstract void ClearSlot(int i);

        public ItemAmount[] GetItemsOfTypes(params ItemType[] types)
        {
            return items.Where(item => !item.IsEmpty && types.Contains(item.Item.GetItemType())).ToArray();
        }

        public void TransferSlotTo(InventorySystem otherInventory, int index)
        {
            var item = items[index];
            otherInventory.AddItem(item.Item, item.Amount);
            ClearSlot(index);
        }

        public void SortItemsByType(ItemType type)
        {
            items = items.Where(item => !item.IsEmpty) // Filtra los ítems vacíos.
                .OrderBy(item => item.Item.GetItemType()) // Ordena primero por ItemType
                .ThenBy(item => item.Item.GetItemName()) // Luego, ordena por nombre de ítem
                .ToList();
            UpdateAllHud();
        }

        protected int StackItems(ItemObject itemObject, int amount)
        {
            if (itemObject.GetStack() <= 1) return amount;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.CanStackWith(itemObject))
                {
                    amount = item.AddAmount(amount);
                    items[i] = item;

                    UpdateHud(i);

                    if (amount <= 0)
                        return 0;
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

                if (!item.IsEmpty && item.Item == itemObject)
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

                    UpdateHud(i);

                    if (amount <= 0) return 0;
                }
            }

            return amount;
        }
    }
}