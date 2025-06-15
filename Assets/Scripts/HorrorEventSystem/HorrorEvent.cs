using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HorrorEvent", menuName = "ScriptableObject/HorrorEvents/Event")]
public class HorrorEvent : ScriptableObject
{
    [Tooltip("List of actions to run when this event triggers")]
    public List<HorrorActionSO> actions = new List<HorrorActionSO>();

    [Tooltip("If true, all actions will start at once; otherwise they run in sequence")]
    public bool runInParallel = false;
}
