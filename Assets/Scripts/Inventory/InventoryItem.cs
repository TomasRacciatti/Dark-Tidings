using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countText;

        [HideInInspector] public Item item;
        [HideInInspector] public int count = 1;
        [HideInInspector] public Transform parentTransform;

        private void Start()
        {
            countText.raycastTarget = false;
            parentTransform = transform.parent;
            if (item)
            {
                SetItem(item);
            }
        }

        public void SetItem(Item newItem, int setCount = 1)
        {
            item = newItem;
            image.sprite = item.image;
            count = setCount;
            RefreshCount();
        }

        public void AddCount(int amount)
        {
            count += amount;
            RefreshCount();
        }

        private void RefreshCount()
        {
            countText.SetText(count.ToString());
            countText.gameObject.SetActive(count > 1);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            parentTransform = transform.parent;
            transform.SetParent(parentTransform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, eventData.position,
                eventData.pressEventCamera, out Vector2 localPoint);
            transform.localPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            if (transform.parent != parentTransform) transform.SetParent(parentTransform);
            transform.localPosition = Vector3.zero;
        }
    }
}