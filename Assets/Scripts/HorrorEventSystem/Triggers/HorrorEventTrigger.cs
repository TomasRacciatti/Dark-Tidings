using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorEventTrigger : MonoBehaviour
{
    [Header("Base Events")] [SerializeField, Tooltip("Add the HorrorEvent you want to fire here")]
    protected HorrorEvent horrorEvent;

    [SerializeField, Tooltip("Scene-driven bindings - GameObjects dependant")]
    protected List<ActionBinding> sceneBindings = new List<ActionBinding>();

    [Header("Delayed Events")] [SerializeField]
    protected float delaySeconds = 0f;
    [SerializeField] protected HorrorEvent delayedHorrorEvent;
    [SerializeField] protected List<ActionBinding> delayedSceneBindings = new List<ActionBinding>();

    [Header("Fire once")] [SerializeField, Tooltip("If true, only fire this event the first time interacted")]
    protected bool fireOnce = true;

    protected bool hasFired = false;

    public event Action FiredEvent;

    protected void Fire()
    {
        if (hasFired && fireOnce) return;

        hasFired = true;
        
        FiredEvent?.Invoke();

        // Si hay bindings hacemos las dos
        if (sceneBindings.Count > 0)
        {
            var all = new List<ActionBinding>(horrorEvent.bindings);
            all.AddRange(sceneBindings);
            EventManager.Instance.TriggerBindings(all, horrorEvent.runInParallel);
        }
        else
        {
            EventManager.Instance.Trigger(horrorEvent);
        }

        if (delayedHorrorEvent != null && delaySeconds > 0f)
        {
            EventManager.Instance.ScheduleDelayed(
                delayedHorrorEvent,
                delayedSceneBindings,
                delaySeconds,
                delayedHorrorEvent.runInParallel
            );
        }
    }
}