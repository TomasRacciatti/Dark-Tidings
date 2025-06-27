using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEventTrigger : HorrorEventTrigger
{
    // Called immediately after the normal IInteractable.Interact().
    public void TryTrigger()
    {
        Fire();
    }
}
