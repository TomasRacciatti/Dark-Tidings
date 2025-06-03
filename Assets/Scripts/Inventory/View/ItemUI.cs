using System;
using Items.Base;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory.View
{
    public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject isEquipable;
        [SerializeField] private TextMeshProUGUI equipableText;

        /*[HideInInspector]*/ public ItemAmount itemAmount;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.sortingOrder = 5;
        }

        public void SetItem(ItemAmount newItemAmount)
        {
            itemAmount = newItemAmount;
            image.sprite = itemAmount.SoItem.Image;
            RefreshCount();
            ValidateEquippable();
        }

        public void SetAmount(int amount)
        {
            itemAmount.SetAmount(amount);
            RefreshCount();
        }

        public void SetEquippable(int slot)
        {
            equipableText.text = slot == -1 ? "E" : (slot + 1).ToString();
        }

        private void ValidateEquippable()
        {
            isEquipable.SetActive(itemAmount.SoItem.IsEquippable);
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
            _canvas.sortingOrder = 15;
            ItemDropper.Show();
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
            _canvas.sortingOrder = 5;
            transform.localPosition = Vector3.zero;
            ItemDropper.Hide();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Debug.Log("Click izquierdo");
                    break;
                case PointerEventData.InputButton.Right:
                    Debug.Log("Click derecho");
                    break;
                case PointerEventData.InputButton.Middle:
                    Debug.Log("Click del botón del medio");
                    SplitItem();
                    break;
            }
        }

        private void SplitItem()
        {
            SlotUI slotUI = GetComponentInParent<SlotUI>();
            slotUI.InventoryUI.InventorySystem.SplitItemStack(slotUI.SlotIndex);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            ItemDescription.Show(itemAmount);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemDescription.Hide();
        }
    }
}