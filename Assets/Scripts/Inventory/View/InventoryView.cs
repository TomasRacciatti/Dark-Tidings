using UnityEngine;

namespace Inventory.View
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] protected InventorySlot[] slots;
        [SerializeField] protected SlotType slotType;

        protected virtual void Awake()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].SetSlotType(slotType, i);
            }
        }

        public void SetItem(int index, ItemAmount itemAmount)
        {
            if (index < 0 || index >= slots.Length) return;

            slots[index].SetItem(itemAmount);
        }
    }
}