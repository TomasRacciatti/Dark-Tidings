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
    [SerializeField] private InteractIconUI _iconPrefab;
    [SerializeField] private int _poolSize = 10;
    
    private List<InteractIconUI> _iconPool = new List<InteractIconUI>();
    private List<InteractIconUI> _activeIcons = new List<InteractIconUI>();
    
    //[SerializeField] private float _directLookThreshold = 0.98f;

    private Camera _mainCamera;
    
    [SerializeField] private InputActionReference _interactAction;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        _mainCamera = Camera.main;
        
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var icon = Instantiate(_iconPrefab, _canvas.transform);
            icon.gameObject.SetActive(false);
            _iconPool.Add(icon);
        }
    }
    
    public void ShowInteractables(List<IInteractable> interactables, IInteractable directInteractable)
    {
        ClearIcons();

        foreach (var interactable in interactables)
        {
            InteractIconUI iconUI = GetIconFromPool();
            iconUI.Initialize(interactable.InteractionPoint);

            if (interactable == directInteractable)
            {
                iconUI.ShowKey(GetInteractKeyText());
            }

            _activeIcons.Add(iconUI);
        }
    }
    
    private InteractIconUI GetIconFromPool()
    {
        foreach (var icon in _iconPool)
        {
            if (!icon.gameObject.activeSelf)
            {
                icon.gameObject.SetActive(true);
                return icon;
            }
        }

        // Por si se me llega a acabar la pool
        var newIcon = Instantiate(_iconPrefab, _canvas.transform);
        _iconPool.Add(newIcon);
        newIcon.gameObject.SetActive(true);
        return newIcon;
    }
    
    private void ClearIcons()
    {
        foreach (var icon in _activeIcons)
        {
            icon.Clear();
        }
        _activeIcons.Clear();
    }
    
    private string GetInteractKeyText()
    {
        if (_interactAction.action.bindings.Count > 0)
        {
            var binding = _interactAction.action.bindings[0];
            return binding.ToDisplayString();
        }

        else
            return "E";
    }
    
    public void HideAll()
    {
        ClearIcons();
    }
}
