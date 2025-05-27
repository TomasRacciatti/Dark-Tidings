using System;
using Items.Base;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.View
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject isEquipable;
        [SerializeField] private TextMeshProUGUI equipableText;
        [SerializeField] private TextMeshProUGUI tooltipText;

        /*[HideInInspector]*/ public ItemAmount itemAmount;
        /*[HideInInspector]*/ public InventoryItem originalItem;
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvas.sortingOrder = 5;
        }

        private void Start()
        {
            amountText.raycastTarget = false;
        }

        private void OnDisable()
        {
            if (tooltipText != null)
            {
                tooltipText.gameObject.SetActive(false);
            }
        }

        public void SetItem(ItemAmount newItemAmount, InventoryItem original = null)
        {
            itemAmount = newItemAmount;
            image.sprite = itemAmount.SoItem.Image;
            RefreshCount();
            ValidateEquipable();
            originalItem = original;
        }

        public void SetAmount(int amount)
        {
            itemAmount.SetAmount(amount);
            RefreshCount();
        }

        public void SetEquipable(int slot)
        {
            equipableText.text = slot == -1 ? "E" : (slot + 1).ToString();
        }
        
        public int GetEquipableSlot()
        {
            if (equipableText.text == "E")
                return -1;

            if (int.TryParse(equipableText.text, out int value))
                return value - 1;

            return -1;
        }

        private void ValidateEquipable()
        {
            if (itemAmount.SoItem.IsEquippable) isEquipable.SetActive(true);
            else isEquipable.SetActive(false);
        }

        private void RefreshCount()
        {
            amountText.SetText(itemAmount.Amount.ToString());
            amountText.gameObject.SetActive(itemAmount.Amount > 1);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
            transform.localPosition = Vector3.zero;
        }

        public void SetRaycast(bool value)
        {
            image.raycastTarget = value;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            canvas.sortingOrder = 15;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera, out Vector2 localPoint);
            transform.localPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            canvas.sortingOrder = 5;
            transform.localPosition = Vector3.zero;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Debug.Log("Clic izquierdo");
                    break;
                case PointerEventData.InputButton.Right:
                    Debug.Log("Clic derecho");
                    break;
                case PointerEventData.InputButton.Middle:
                    Debug.Log("Clic del botón del medio");
                    break;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipText != null)
            {
                tooltipText.text = itemAmount.ItemName;
                tooltipText.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltipText != null)
            {
                tooltipText.gameObject.SetActive(false);
            }
        }
    }
}