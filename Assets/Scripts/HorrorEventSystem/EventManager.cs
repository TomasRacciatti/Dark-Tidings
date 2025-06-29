using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    
    public static event Action<HorrorEvent> OnEventCompleted;
    
    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);
    }

    
    public void Trigger(HorrorEvent horrorEvent)
    {
        if (horrorEvent.runInParallel)
            StartCoroutine(RunParallelThenNotify(horrorEvent));
        else
            StartCoroutine(RunSequentialThenNotify(horrorEvent));
    }
    
    private IEnumerator RunSequentially(HorrorEvent horrorEvent)
    {
        foreach (var bind in horrorEvent.bindings)
            yield return StartCoroutine(RunBinding(bind));
    }
    
    private IEnumerator RunSequentialThenNotify(HorrorEvent horrorEvent)
    {
        foreach (var bind in horrorEvent.bindings)
            yield return StartCoroutine(RunBinding(bind));

        OnEventCompleted?.Invoke(horrorEvent);
    }
    
    private IEnumerator RunParallelThenNotify(HorrorEvent horrorEvent)
    {
        foreach (var bind in horrorEvent.bindings)
            StartCoroutine(RunBinding(bind));

        yield return null;

        OnEventCompleted?.Invoke(horrorEvent);
    }
    
    
    public void TriggerBindings(IEnumerable<ActionBinding> bindings, bool runInParallel)
    {
        if (runInParallel)
        {
            foreach (var bind in bindings)
                StartCoroutine(RunBinding(bind));
        }
        else
        {
            StartCoroutine(RunSequentialBindings(bindings));
        }
    }
    
    private IEnumerator RunSequentialBindings(IEnumerable<ActionBinding> bindings)
    {
        foreach (var bind in bindings)
            yield return StartCoroutine(RunBinding(bind));
    }

    private IEnumerator RunBinding(ActionBinding bind)
    {
        // SOs que usan target
        if (bind.actionDef is LightToggleAction lta)
        {
            foreach (var lightObject in bind.targets)
            {
                var lightController = lightObject?.GetComponent<LightController>();
                yield return StartCoroutine(lta.ExecuteOn(lightController));
            }
            yield break;
        }

        else if (bind.actionDef is ToggleGameObjectAction tga)
        {
            if (tga.mode == ToggleGameObjectAction.TargetMode.ByReference)
            {
                foreach (var target in bind.targets)
                    yield return StartCoroutine(tga.ExecuteOn(target));
                yield break;
            }
            else
            {
                yield return StartCoroutine(tga.Execute());
            }
        }
        
        else if (bind.actionDef is TransportAction transportAction)
        {
            if (bind.targets.Count >= 2)
                yield return StartCoroutine(transportAction.ExecuteOn(bind.targets[0], bind.targets[1]));
            
            yield break;    
        }
        
        else if (bind.actionDef is DoorStateAction doorState)
        {
            foreach (var targetDoor in bind.targets)
                yield return StartCoroutine(doorState.ExecuteOn(targetDoor));
            yield break;
        }
        
        if (bind.actionDef is ActivateEnemyAction activateEnemy)
        {
            foreach (var target in bind.targets)
                yield return StartCoroutine(activateEnemy.ExecuteOn(target));
            yield break;
        }
        // Agregar acciones que requieren un target (ExecuteOn) aca

        // Si no tienen target como es el caso del play dialogue
        else
        {
            yield return StartCoroutine(bind.actionDef.Execute());
        }
    }
}
