using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private float detectionRange = 3f;
    [SerializeField] private float directLookThreshold = 0.98f; // Si esta en 1, significa que esta mirando directo

    private Camera playerCamera;
    private IInteractable currentInteractable;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        DetectInteractable();
    }

    private void DetectInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, detectionRange))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                if (interactable != currentInteractable)
                {
                    currentInteractable = interactable;
                }

                InteractionUIManager.Instance.UpdateUI(interactable, CalculateAlignment(interactable));
                return;
            }
        }
        
        if (currentInteractable != null)
        {
            currentInteractable = null;
            InteractionUIManager.Instance.HideUI();
        }
    }
    
    
    private float CalculateAlignment(IInteractable interactable)
    {
        Vector3 directionToTarget = (interactable.InteractionPoint.position - playerCamera.transform.position).normalized;
        float dot = Vector3.Dot(playerCamera.transform.forward, directionToTarget);
        return dot; // 1 = perfect alignment
    }
}
