using Characters.Player;
using Inventory.View;
using Items.Base;
using Managers;
using UnityEngine;

namespace Inventory
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private InventorySlot[] _slots;
        [SerializeField] private int _selectedSlot = 0;
        [SerializeField] private InventorySlot _craftedSlot;
        [SerializeField] private GameObject slotSelector;
        [SerializeField] private SO_Item bullet;

        private void Awake()
        {
            ChangeSelectedSlot(_selectedSlot);
        }
        
        public void ChangeSelectedSlot(int slot)
        {
            if (_selectedSlot == slot) return;
            if (slot >= _slots.Length) return;

            _selectedSlot = slot;
            slotSelector.transform.SetParent(_slots[_selectedSlot].transform, false);
            slotSelector.transform.localPosition = Vector3.zero;
        }

        public void SetItem(ItemAmount itemAmount)
        {
            InventoryItem item = _slots[_selectedSlot].GetComponentInChildren<InventoryItem>();

            if (item == null)
            {
                GameObject itemobject = Instantiate(GameManager.Canvas.inventoryManager.itemSlotPrefab, _slots[_selectedSlot].transform);
                item = itemobject.GetComponent<InventoryItem>();
                item.SetRaycast(false);
            }
            
            item.SetItem(itemAmount);
            ChangeSelectedSlot(_selectedSlot + 1);
            UpdateCrafting();
        }

        public void Clear()
        {
            foreach (var slot in _slots)
            {
                InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            ChangeSelectedSlot(0);
            _craftedSlot.SetItem(new ItemAmount(null, 0));
        }

        private void UpdateCrafting()
        {
            var item0 = _slots[0].GetItemObject();
            var item1 = _slots[1].GetItemObject();
            var item2 = _slots[2].GetItemObject();

            // Condición base: ambos ítems existen y son iguales
            bool baseValid = item0 != null && item1 != null && item0 == item1;

            // Condición modificador válido: item2 es null o distinto del base
            bool modifierValid = item2 == null || item2 != item0;

            if (baseValid && modifierValid)
            {
                var bullet = new ItemAmount(this.bullet, 5);
                bullet.AddModifier(new ItemAmount(item2 != null ? item2 : item0, 1));
                _craftedSlot.SetItem(bullet);
            }
            else
            {
                _craftedSlot.SetItem(new ItemAmount(null, 0));
            }
        }


        public void CraftItem()
        {
            ItemAmount item = _craftedSlot.GetItemAmount();
            if (item.IsEmpty)
            {
                return;
            }
            var slotData = new[] {
                new { Item = _slots[0].GetItemObject(), Index = 0 },
                new { Item = _slots[1].GetItemObject(), Index = 1 },
                new { Item = _slots[2].GetItemObject(), Index = 2 }
            };
            
            GameManager.Player.inventory.AddItem(_craftedSlot.GetItemAmount());
        }
    }
}
