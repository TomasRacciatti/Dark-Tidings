using System;
using Characters.Player;
using Inventory.Controller;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.View
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject isEquipable;
        [SerializeField] private TextMeshProUGUI equipableText;

        [HideInInspector] public ItemAmount itemAmount;
        [HideInInspector] public Transform parentTransform;
        [HideInInspector] public InventoryItem originalItem;
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvas.sortingOrder = 5;
        }

        private void Start()
        {
            amountText.raycastTarget = false;
            parentTransform = transform.parent;
        }

        public void SetItem(ItemObject newItemObject, int amount, InventoryItem original = null)
        {
            itemAmount.SetItem(newItemObject, amount);
            image.sprite = itemAmount.Item.GetImage;
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

        private void ValidateEquipable()
        {
            if (itemAmount.Item.IsEquippable) isEquipable.SetActive(true);
            else isEquipable.SetActive(false);
        }

        private void RefreshCount()
        {
            amountText.SetText(itemAmount.Amount.ToString());
            amountText.gameObject.SetActive(itemAmount.Amount > 1);
        }

        public void SetParent()
        {
            transform.SetParent(parentTransform);
            transform.localPosition = Vector3.zero;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvas.sortingOrder = 15;
            image.raycastTarget = false;
            parentTransform = transform.parent;
            transform.SetParent(parentTransform);
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

            if (parentTransform == transform.parent) //verifica que no es el mismo
            {
                transform.localPosition = Vector3.zero;
                return;
            }

            InventorySlot originalSlot = transform.parent.GetComponent<InventorySlot>();
            InventorySlot targetSlot = parentTransform.GetComponent<InventorySlot>();

            InventoryItem toItem = targetSlot.GetComponentInChildren<InventoryItem>();

            if (targetSlot == null) //verificar que el target no es nulo
            {
                transform.localPosition = Vector3.zero;
                return;
            }

            if (originalSlot.slotType == targetSlot.slotType)
            {
                int fromIndex = originalSlot.slotIndex;
                int toIndex = targetSlot.slotIndex;
                SetParent();
                if (toItem) toItem.SetParent();
                bool itemCleared = PlayerController.Instance.inventory.SwapItems(fromIndex, toIndex);
                if (itemCleared) Destroy(gameObject);
                return;
            }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Debug.Log("Clic izquierdo");/*
                    if (itemAmount.Item.GetItemType == Items.ItemType2.Weapon)
                    {
                        //Toolbar.Instance.EquipItem(itemAmount, 0);
                        ItemsInHand.Instance.ActivateGlock();
                    }*/

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