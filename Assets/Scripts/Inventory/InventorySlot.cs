using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Inventory
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