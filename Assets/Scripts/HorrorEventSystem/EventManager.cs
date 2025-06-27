using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    
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
        {
            foreach (var bind in horrorEvent.bindings)
                StartCoroutine(RunBinding(bind));
        }
        else
        {
            StartCoroutine(RunSequentially(horrorEvent));
        }
    }
    
    private IEnumerator RunSequentially(HorrorEvent horrorEvent)
    {
        foreach (var bind in horrorEvent.bindings)
            yield return StartCoroutine(RunBinding(bind));
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
        // if this SO needs a target, handle it here
        if (bind.actionDef is LightToggleAction lta)
        {
            foreach (var lightObject in bind.targets)
            {
                var lightController = lightObject?.GetComponent<LightController>();
                yield return StartCoroutine(lta.ExecuteOn(lightController));
            }
        }

        else if (bind.actionDef is ToggleGameObjectAction tga)
        {
            if (tga.mode == ToggleGameObjectAction.TargetMode.ByReference)
            {
                foreach (var go in bind.targets)
                    yield return StartCoroutine(tga.ExecuteOn(go));
            }
            else
            {
                yield return StartCoroutine(tga.Execute());
            }
        }
        // Agregar acciones que requieren un target (ExecuteOn) aca

        // Si no tienen target como es el caso del play dialogue
        else
        {
            yield return StartCoroutine(bind.actionDef.Execute());
        }
    }
}
