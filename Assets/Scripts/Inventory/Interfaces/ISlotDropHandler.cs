using Inventory.View;

namespace Inventory.Interfaces
{
    public interface ISlotDropHandler
    {
        void HandleDrop(SlotUI fromSlot, SlotUI toSlot, ItemUI fromItemUI, ItemUI toItemUI);
        bool CanHandle(SlotType fromSlotType, SlotType toSlotType);
    }
    

    public class InventoryToToolbarHandler : ISlotDropHandler
    {
        public bool CanHandle(SlotType from, SlotType to)
        {
            return from.name == "Inventory" && to.name == "Toolbar";
        }

        public void HandleDrop(SlotUI fromSlot, SlotUI toSlot, ItemUI fromItemUI, ItemUI toItemUI)
        {
            // Lógica de mover item de inventario a toolbar y equiparlo
        }
    }
}