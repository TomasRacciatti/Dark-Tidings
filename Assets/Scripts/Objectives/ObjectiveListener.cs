using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HorrorEventTrigger))]
public class ObjectiveListener : MonoBehaviour
{
    [Tooltip("Objective to show when the trigger fires")]
    [TextArea] public string objectiveText;
    
    private HorrorEventTrigger _trigger;

    private void Awake()
    {
        _trigger = GetComponent<HorrorEventTrigger>();
    }

    private void OnEnable()
    {
        _trigger.FiredEvent += OnFired;
    }

    private void OnDisable()
    {
        _trigger.FiredEvent -= OnFired;
    }

    private void OnFired()
    {
        ObjectiveManager.Instance.SetObjective(objectiveText);
    }
}
