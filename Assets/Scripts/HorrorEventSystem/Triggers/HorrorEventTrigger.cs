using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorEventTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("Add the HorrorEvent you want to fire here")]
    protected HorrorEvent horrorEvent;
    
    [SerializeField, Tooltip("Scene-driven bindings - GameObjects dependant")]
    protected List<ActionBinding> sceneBindings = new List<ActionBinding>();
    
    [SerializeField, Tooltip("If true, only fire this event the first time interacted")]
    protected bool fireOnce = true;

    protected bool hasFired = false;
    
    protected void Fire()
    {
        if (hasFired && fireOnce) return;
        
        hasFired = true;
        
        // Si hay bindings hacemos las dos
        if (sceneBindings != null && sceneBindings.Count > 0)
        {
            var all = new List<ActionBinding>(horrorEvent.bindings);
            all.AddRange(sceneBindings);
            EventManager.Instance.TriggerBindings(all, horrorEvent.runInParallel);
        }
        else
        {
            EventManager.Instance.Trigger(horrorEvent);
        }
    }
}
