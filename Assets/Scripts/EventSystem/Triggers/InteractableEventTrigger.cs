using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEventTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("Add the HorrorEvent you want to fire here")]
    private HorrorEvent interactEvent;
    
    [SerializeField, Tooltip("If true, only fire this event the first time interacted")]
    private bool fireOnce = true;

    private bool hasFired = false;
    
    
    // Called immediately after the normal IInteractable.Interact().
    public void TryTrigger()
    {
        if (interactEvent == null) return;
        if (fireOnce && hasFired) return;

        EventManager.Instance.Trigger(interactEvent);
        hasFired = true;
    }
}
