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

        public void SetItem(ItemAmount itemAmount, InventoryItem original = null)
        {
            itemAmount.SetItem(itemAmount);
            image.sprite = itemAmount.Item.Image;
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

            if (parentTransform == transform.parent)
            {
                transform.localPosition = Vector3.zero;
                return;
            }

            InventorySlot originalSlot = transform.parent.GetComponent<InventorySlot>();
            InventorySlot targetSlot = parentTransform.GetComponent<InventorySlot>();

            if (targetSlot == null)
            {
                transform.localPosition = Vector3.zero;
                return;
            }

            InventoryItem toItem = targetSlot.GetComponentInChildren<InventoryItem>();

            if (HandleInventoryToInventory(originalSlot, targetSlot, toItem)) return;
            if (HandleToolbarToToolbar(originalSlot, targetSlot, toItem)) return;
            if (HandleToolbarToInventory(originalSlot, targetSlot, toItem)) return;
            if (HandleInventoryToToolbar(originalSlot, targetSlot, toItem)) return;

            transform.localPosition = Vector3.zero;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Debug.Log("Clic izquierdo"); /*
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

        private bool HandleInventoryToInventory(InventorySlot originalSlot, InventorySlot targetSlot,
            InventoryItem toItem)
        {
            if (originalSlot.slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType ||
                targetSlot.slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType)
                return false;

            int fromIndex = originalSlot.slotIndex;
            int toIndex = targetSlot.slotIndex;

            SetParent();
            if (toItem) toItem.SetParent();

            bool itemCleared = PlayerController.Instance.inventory.SwapItems(fromIndex, toIndex);
            if (itemCleared)
                Destroy(gameObject);

            return true;
        }

        private bool HandleToolbarToToolbar(InventorySlot originalSlot, InventorySlot targetSlot, InventoryItem toItem)
        {
            if (originalSlot.slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType ||
                targetSlot.slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType)
                return false;

            SetParent();
            SetEquipable(targetSlot.slotIndex);
            if (originalItem != null)
                originalItem.SetEquipable(targetSlot.slotIndex);

            if (toItem)
            {
                toItem.SetParent();
                toItem.SetEquipable(originalSlot.slotIndex);
                if (toItem.originalItem != null)
                    toItem.originalItem.SetEquipable(originalSlot.slotIndex);
            }

            return true;
        }

        private bool HandleToolbarToInventory(InventorySlot originalSlot, InventorySlot targetSlot,
            InventoryItem toItem)
        {
            if (originalSlot.slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType ||
                targetSlot.slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType)
                return false;

            SetEquipable(-1);
            if (originalItem != null)
                originalItem.SetEquipable(-1);

            SetParent();

            return true;
        }

        private bool HandleInventoryToToolbar(InventorySlot originalSlot, InventorySlot targetSlot, InventoryItem toItem)
        {
            // Verificar si el originalSlot es un Inventario y el targetSlot es una Toolbar
            if (originalSlot.slotType != CanvasGameManager.Instance.inventoryManager.inventorySlotType ||
                targetSlot.slotType != CanvasGameManager.Instance.inventoryManager.toolbarSlotType)
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
                originalSlot.SetInventoryItem(null);

                return true;
            }

            // Si no existe en la Toolbar, crear un nuevo item en el targetSlot
            SetParent();
            targetSlot.SetInventoryItem(this);
            SetEquipable(targetSlot.slotIndex);

            return true;
        }
    }
}