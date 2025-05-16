using Characters.Player;
using Inventory.View;
using Items.Base;
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
                GameObject itemobject = Instantiate(CanvasManager.Instance.inventoryManager.itemSlotPrefab, _slots[_selectedSlot].transform);
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
            var slotData = new[] {
                new { Item = _slots[0].GetItemObject(), Index = 0 },
                new { Item = _slots[1].GetItemObject(), Index = 1 },
                new { Item = _slots[2].GetItemObject(), Index = 2 }
            };

            ItemAmount bullet = new ItemAmount(this.bullet, 1);
            
            if (slotData[0].Item != null && slotData[1].Item != null)
            {
                if (slotData[2].Item != null)
                {
                    if (slotData[2].Item != slotData[1].Item && slotData[2].Item != slotData[0].Item)
                    {
                        bullet.AddModifier(slotData[2].Item);
                        _craftedSlot.SetItem(bullet);
                    }
                    else
                    {
                        _craftedSlot.SetItem(new ItemAmount(null, 0));
                    }
                }
                else
                {
                    if (slotData[0].Item == slotData[1].Item)
                    {
                        bullet.AddModifier(slotData[0].Item);
                        _craftedSlot.SetItem(bullet);
                    }
                    else
                    {
                        _craftedSlot.SetItem(new ItemAmount(null, 0));
                    }
                }
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
            
            PlayerController.Instance.inventory.AddItem(_craftedSlot.GetItemAmount());
        }
    }
}
