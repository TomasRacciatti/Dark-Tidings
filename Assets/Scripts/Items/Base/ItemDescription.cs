﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Items.Base
{
    public class ItemDescription : MonoBehaviour
    {
        private static ItemDescription _instance;
        
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text itemDescriptionText;
        [SerializeField] private Image itemIcon;
        
        private RectTransform _rectTransform;
        private Canvas _canvas;

        private void Awake()
        {
            _instance = this;
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            Hide();
        }

        public static void Show(ItemAmount itemAmount)
        {
            _instance.itemNameText.text = itemAmount.ItemName;
            _instance.itemDescriptionText.text = itemAmount.Description;
            _instance.itemIcon.sprite = itemAmount.SoItem.Image;
            _instance.gameObject.SetActive(true);
            _instance.UpdatePositionAndPivot();
        }

        public static void Hide()
        {
            _instance.gameObject.SetActive(false);
        }
        

        private void Update()
        {
            if (!gameObject.activeSelf) return;

            UpdatePositionAndPivot(); // Seguir actualizando por si el mouse se mueve
        }

        private void AdjustPivotToStayOnScreen(Vector2 screenPos)
        {
            Vector2 pivot = new Vector2(
                screenPos.x / Screen.width < 0.5f ? 0f : 1f,
                screenPos.y / Screen.height < 0.5f ? 0f : 1f
            );

            _rectTransform.pivot = pivot;
        }
        
        private void UpdatePositionAndPivot()
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

            AdjustPivotToStayOnScreen(mouseScreenPos);

            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                mouseScreenPos,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out anchoredPos);

            _rectTransform.anchoredPosition = anchoredPos;
        }
    }
}