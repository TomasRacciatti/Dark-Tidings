using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectiveListener : MonoBehaviour
{
    [Serializable]
    private struct Entry
    {
        [Tooltip("Which trigger to watch")]
        public HorrorEventTrigger trigger;
        [TextArea, Tooltip("Objective text to set when that trigger fires")]
        public string objectiveText;
    }
    
    [Tooltip("Drag each VolumeTrigger / InteractableEventTrigger you want to listen to here")]
    [SerializeField]
    private List<Entry> entries = new List<Entry>();

    private void OnEnable()
    {
        foreach (var e in entries)
        {
            if (e.trigger != null)
                e.trigger.FiredEvent += () => ObjectiveManager.Instance.SetObjective(e.objectiveText);
        }
    }

    private void OnDisable()
    {
        foreach (var e in entries)
        {
            if (e.trigger != null)
                e.trigger.FiredEvent -= () => OnFired(e.objectiveText);
        }
    }

    private void OnFired(string objective)
    {
        ObjectiveManager.Instance.SetObjective(objective);
    }
}
