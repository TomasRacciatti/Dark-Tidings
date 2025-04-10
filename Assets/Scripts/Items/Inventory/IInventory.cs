using System.Collections.Generic;

namespace Inventory
{
    public interface IInventory
    {
        void AddItem(object item);
        bool RemoveItem(object item);
        List<object> GetItems();
        bool HasItem(object item);
        void Clear();
    }
}