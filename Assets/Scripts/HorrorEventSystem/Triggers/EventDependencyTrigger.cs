using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDependencyTrigger : HorrorEventTrigger
{
    [Tooltip("These events must all complete before we fire our HorrorEvent")]
    [SerializeField] private List<HorrorEvent> requiredEvents = new List<HorrorEvent>();

    private HashSet<HorrorEvent> _finished = new HashSet<HorrorEvent>();
    
    private void OnEnable()
    {
        _finished.Clear();
        EventManager.OnEventCompleted += HandleCompleted;
    }
    
    private void OnDisable()
    {
        EventManager.OnEventCompleted -= HandleCompleted;
    }
    
    private void HandleCompleted(HorrorEvent horrorEvent)
    {
        if (requiredEvents.Contains(horrorEvent))
        {
            _finished.Add(horrorEvent);
            // once we’ve seen *all* prerequisites:
            if (_finished.Count >= requiredEvents.Count)
            {
                Fire();
            }
        }
    }
}
