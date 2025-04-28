using UnityEngine;
using UnityEngine.EventSystems;
using Characters.Player;
namespace Inventory.View
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public SlotType slotType;
        public int slotIndex;

        private InventoryItem _inventoryItem;

        public void SetSlotType(SlotType type, int index)
        {
            slotType = type;
            slotIndex = index;
        }

        public void SetItem(ItemAmount itemAmount)
        {
            if (itemAmount.IsEmpty)
            {
                if (_inventoryItem != null)
                {
                    Destroy(_inventoryItem.gameObject);
                    _inventoryItem = null;
                }
                return;
            }

            if (_inventoryItem == null)
            {
                GameObject newItem = Instantiate(CanvasGameManager.Instance.inventoryManager.itemPrefab, transform);
                _inventoryItem = newItem.GetComponent<InventoryItem>();
            }
            
            _inventoryItem.SetItem(itemAmount);
        }
        
        public void SetInventoryItem(InventoryItem inventoryItem)
        {
            _inventoryItem = inventoryItem;

            if (inventoryItem != null)
            {
                inventoryItem.parentTransform = transform;
            }
        }


        public void OnDrop(PointerEventData eventData)
        {
            InventoryItem fromItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (fromItem == null)
            {
                transform.localPosition = Vector3.zero;
                return;
            }
            
            if (fromItem.parentTransform == transform)
            {
                transform.localPosition = Vector3.zero;
                return;
            }

            InventorySlot fromSlot = fromItem.GetComponentInParent<InventorySlot>();
            InventoryItem toItem = _inventoryItem;

            if (HandleInventoryToInventory(fromSlot, fromItem, toItem)) return;/*
            if (HandleToolbarToToolbar(fromSlot, fromItem, toItem)) return;
            if (HandleToolbarToInventory(fromSlot, fromItem, toItem)) return;
            if (HandleInventoryToToolbar(fromSlot, fromItem, toItem)) return;*/
        }
        
        private bool HandleInventoryToInventory(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType ||
                slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType)
                return false;
            PlayerController.Instance.inventory.SwapItems(fromSlot.slotIndex, slotIndex);
            return true;
        }
        /*
        private bool HandleToolbarToToolbar(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType ||
                slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType)
                return false;

            SetParent();
            SetEquipable(toSlot.slotIndex);
            if (originalItem != null)
                originalItem.SetEquipable(toSlot.slotIndex);

            if (toItem)
            {
                toItem.SetParent();
                toItem.SetEquipable(fromSlot.slotIndex);
                if (toItem.originalItem != null)
                    toItem.originalItem.SetEquipable(fromSlot.slotIndex);
            }

            return true;
        }

        private bool HandleToolbarToInventory(InventorySlot fromSlot, InventoryItem fromItem,
            InventoryItem toItem)
        {
            if (fromSlot.slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType ||
                slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType)
                return false;

            SetEquipable(-1);
            if (originalItem != null)
                originalItem.SetEquipable(-1);

            SetParent();

            return true;
        }

        private bool HandleInventoryToToolbar(InventorySlot fromSlot, InventoryItem fromItem, InventoryItem toItem)
        {
            // Verificar si el originalSlot es un Inventario y el targetSlot es una Toolbar
            if (fromSlot.slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType ||
                slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType)
                return false;

            // Buscar si el item ya existe en la Toolbar
            InventorySlot existingSlot = CanvasGameManager.Instance.inventoryManager.toolbar.GetItemSlot(this);

            print(existingSlot);
            if (existingSlot != null)
            {
                // Si ya existe, mover el item a este slot en la Toolbar
                SetParent();
                existingSlot.SetInventoryItem(this);
                SetEquipable(existingSlot.slotIndex);

                // Vaciar el slot original en el Inventario
                fromSlot.SetInventoryItem(null);

                return true;
            }

            // Si no existe en la Toolbar, crear un nuevo item en el targetSlot
            SetParent();
            toSlot.SetInventoryItem(this);
            SetEquipable(toSlot.slotIndex);

            return true;
        }*/
    }
}