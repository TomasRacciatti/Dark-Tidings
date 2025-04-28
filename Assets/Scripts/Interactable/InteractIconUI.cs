using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractIconUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _keyImage;
    [SerializeField] private TextMeshProUGUI _keyText;
    [SerializeField] private Camera _mainCamera;

    private RectTransform _rectTransform;
    [SerializeField] private Transform _target;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(Transform target)
    {
        _target = target;
        if (_mainCamera == null)
            _mainCamera = Camera.main;
        
        Debug.Log($"Initialized icon with target: {_target?.name}");

        ShowIconOnly();
    }

    private void Update()
    {
        Debug.Log("Running");


        if (_target == null)
            return;

        Debug.Log("Running2");
        
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(_target.position);

        RectTransform canvasRect = _rectTransform.root.GetComponent<RectTransform>();
        
        Debug.Log($"Screen position of {_target.name}: {screenPos}");

        if (screenPos.z > 0)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPos,
                    null,
                    out Vector2 localPoint))
            {
                _rectTransform.anchoredPosition = localPoint;
            }
        }
    }

    public void ShowKey(string keyText)
    {
        _iconImage.enabled = false;
        _keyImage.enabled = true;
        _keyText.enabled = true;
        _keyText.text = keyText;
    }

    public void ShowIconOnly()
    {
        _iconImage.enabled = true;
        _keyImage.enabled = false;
        _keyText.text = string.Empty;
    }

    public void Clear()
    {
        _target = null;
        gameObject.SetActive(false);
    }
}