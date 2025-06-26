using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorEventTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("Add the HorrorEvent you want to fire here")]
    protected HorrorEvent horrorEvent;
    
    [SerializeField, Tooltip("If true, only fire this event the first time interacted")]
    protected bool fireOnce = true;

    protected bool hasFired = false;
    
    protected void Fire()
    {
        if (hasFired && fireOnce) return;
        EventManager.Instance.Trigger(horrorEvent);
        hasFired = true;
    }
}
