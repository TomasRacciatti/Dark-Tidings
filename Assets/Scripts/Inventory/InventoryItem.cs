using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Items;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private bool inToolbar = false;

        [HideInInspector] public ItemAmount itemAmount;
        [HideInInspector] public Transform parentTransform;

        private void Start()
        {
            amountText.raycastTarget = false;
            parentTransform = transform.parent;
        }

        public void SetItem(ItemObject newItemObject, int setCount = 1)
        {
            itemAmount.SetItem(newItemObject, setCount);
            image.sprite = itemAmount.item.image;
            RefreshCount();
        }

        public void AddCount(int amountAdded)
        {
            itemAmount.AddAmount(amountAdded);
            RefreshCount();
        }

        private void RefreshCount()
        {
            amountText.SetText(itemAmount.amount.ToString());
            amountText.gameObject.SetActive(itemAmount.amount > 1);
        }

        public void OnBeginDrag(PointerEventData eventData) //drag and drop poner despues
        {
            /*image.raycastTarget = false;
            parentTransform = transform.parent;
            transform.SetParent(parentTransform);*/
        }

        public void OnDrag(PointerEventData eventData)
        {
            /*RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, eventData.position,
                eventData.pressEventCamera, out Vector2 localPoint);
            transform.localPosition = localPoint;*/
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            /*image.raycastTarget = true;
            if (transform.parent != parentTransform) transform.SetParent(parentTransform);
            transform.localPosition = Vector3.zero;*/
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Debug.Log("Clic izquierdo");
                    if (itemAmount.item.type == ItemType.Weapon)
                    {
                        Toolbar.Instance.EquipItem(itemAmount, 0);
                        ItemsInHand.Instance.ActivateGlock();
                    }
                    break;
                case PointerEventData.InputButton.Right:
                    Debug.Log("Clic derecho");
                    break;
                case PointerEventData.InputButton.Middle:
                    Debug.Log("Clic del botón del medio");
                    break;
            }
        }
    }
}