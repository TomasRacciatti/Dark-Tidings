using System.Collections.Generic;
using Items.Base;

namespace Inventory.Interfaces
{
    public interface IInventoryObserver
    {
        public void OnInventoryChanged(List<ItemAmount> currentItems);
        public void OnItemChanged(int index, ItemAmount newItem);
    }
}