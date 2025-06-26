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

    private IEnumerator RunBinding(ActionBinding bind)
    {
        // if this SO needs a target, handle it here
        if (bind.actionDef is LightToggleAction lta)
        {
            var light = bind.target?.GetComponent<Light>(); // Esto en realidad va ser el game object porque voy a acceder al codigo y cambiar el enum
            yield return StartCoroutine(lta.ExecuteOn(light));
        }
        // Agregar acciones que requieren un target (ExecuteOn) aca

        // Si no tienen target como es el caso del play dialogue
        else
        {
            yield return StartCoroutine(bind.actionDef.Execute());
        }
    }
}
