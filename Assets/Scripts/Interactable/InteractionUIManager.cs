using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interfaces;
using TMPro;
using UnityEngine.InputSystem;

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance { get; private set; }
    
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _interactionIcon;
    [SerializeField] private Image _interactionKey;
    [SerializeField] private float _directLookThreshold = 0.98f;
    [SerializeField] private float _interactionIconThreshold  = 0.75f;

    private Camera _mainCamera;
    private IInteractable _currentTarget;
    
    [SerializeField] private InputActionReference _interactAction;
    [SerializeField] private TextMeshProUGUI _interactionKeyText;
    private char _defaultKey = 'E';
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        _mainCamera = Camera.main;
        
        HideUI();
    }

    private void Update()
    {
        if (_currentTarget != null)
        {
            UpdatePosition(_currentTarget.InteractionPoint);
        }
    }
    
    public void UpdateUI(IInteractable interactable, float alignment)
    {
        _currentTarget = interactable;

        bool isDirectlyLooking = alignment >= _directLookThreshold;
        bool isCloseLooking = alignment >= _interactionIconThreshold;

        _interactionIcon.enabled = isCloseLooking && !isDirectlyLooking;
        _interactionKey.enabled = isDirectlyLooking;

        if (!isDirectlyLooking)
        {
            _interactionKeyText.text = string.Empty;
        }
        else
        {
            _UpdateInteractionKeyUI();
        }

        _interactionIcon.rectTransform.position = _mainCamera.WorldToScreenPoint(interactable.InteractionPoint.position);
        _interactionKey.rectTransform.position = _mainCamera.WorldToScreenPoint(interactable.InteractionPoint.position);
    }
    
    
    private void _UpdateInteractionKeyUI()
    {
        if (_interactAction == null || _interactAction.action == null)
        {
            Debug.LogWarning("Interact Action is not assigned!");
            return;
        }

        if (_interactAction.action.bindings.Count > 0)
        {
            var binding = _interactAction.action.bindings[0];
            _interactionKeyText.text = binding.ToDisplayString();
        }
        else
        {
            Debug.LogWarning("Interact Action has no bindings!");
        }
    }

    public void HideUI()
    {
        _interactionIcon.enabled = false;
        _interactionKey.enabled = false;
        _interactionKeyText.text = string.Empty;
        _currentTarget = null;
    }

    private void UpdatePosition(Transform target)
    {
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(target.position);
        _interactionIcon.transform.position = screenPos;
    }
}
