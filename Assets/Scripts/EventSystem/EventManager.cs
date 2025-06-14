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
            foreach (var action in horrorEvent.actions)
                StartCoroutine(action.Execute());
        }
        else
        {
            StartCoroutine(RunSequentially(horrorEvent));
        }
    }
    
    private IEnumerator RunSequentially(HorrorEvent horrorEvent)
    {
        foreach (var action in horrorEvent.actions)
            yield return StartCoroutine(action.Execute());
    }
}
