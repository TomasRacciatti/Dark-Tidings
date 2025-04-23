using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.View
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (!GetComponentInChildren<InventoryItem>())
            {
                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
                item.parentTransform = transform;
            }
        }
    }
}