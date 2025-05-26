using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private float _sphereRadius = 0.75f;
    [SerializeField] private float _detectionRange = 3f;
    [SerializeField] private LayerMask _interactableLayerMask;

    private Camera _playerCamera;
    private IInteractable _currentInteractable;

    private void Awake()
    {
        _playerCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        DetectInteractable();
    }

    private void DetectInteractable()
    {
        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, _sphereRadius, out hit, _detectionRange, _interactableLayerMask))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                if (_currentInteractable != interactable)
                {
                    _currentInteractable = interactable;
                }

                if (_currentInteractable != null)
                {
                    InteractionUIManager.Instance.UpdateUI(interactable, CalculateAlignment(interactable));
                }

                return;
            }
        }

        if (_currentInteractable != null)
        {
            _currentInteractable = null;
            InteractionUIManager.Instance.HideUI();
        }
    }


    private float CalculateAlignment(IInteractable interactable)
    {
        Vector3 directionToTarget =
            (interactable.InteractionPoint.position - _playerCamera.transform.position).normalized;
        float dot = Vector3.Dot(_playerCamera.transform.forward, directionToTarget);
        return dot;
    }
}