using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 3f;
    [SerializeField] private float _sphereRadius = 0.5f;
    [SerializeField] private LayerMask _interactableLayerMask;
    [SerializeField] private float _directLookThreshold = 0.98f; // Si esta en 1, significa que esta mirando directo

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
        RaycastHit[] hits = Physics.SphereCastAll(ray, _sphereRadius, _detectionRange, _interactableLayerMask);

        List<IInteractable> foundInteractables = new List<IInteractable>();

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                foundInteractables.Add(interactable);
            }
        }

        if (foundInteractables.Count > 0)
        {
            IInteractable bestInteractable = GetBestInteractable(foundInteractables);
            InteractionUIManager.Instance.ShowInteractables(foundInteractables, bestInteractable);
        }
        else
        {
            InteractionUIManager.Instance.HideAll();
        }
    }
    
    
    private IInteractable GetBestInteractable(List<IInteractable> interactables) // Helper function para solo ponerle la E al mejor
    {
        IInteractable best = null;
        float bestAlignment = -1f;

        foreach (var interactable in interactables)
        {
            float alignment = CalculateAlignment(interactable);
            if (alignment > bestAlignment)
            {
                bestAlignment = alignment;
                best = interactable;
            }
        }

        return best;
    }
    
    private float CalculateAlignment(IInteractable interactable)
    {
        Vector3 directionToTarget = (interactable.InteractionPoint.position - _playerCamera.transform.position).normalized;
        float dot = Vector3.Dot(_playerCamera.transform.forward, directionToTarget);
        return dot; 
    }
}
