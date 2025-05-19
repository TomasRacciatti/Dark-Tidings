using Items.Base;

namespace Interfaces
{
    public interface IInventoryListener
    {
        void OnItemUpdated(int index, ItemAmount itemAmount);
        void OnInventoryUpdated();
    }
}