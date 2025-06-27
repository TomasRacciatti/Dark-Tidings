using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ActionBinding // ActionBinding nos va a hacer la conexion entre la accion y el game object que afecta
{
    [Tooltip("Which action to run (shared SO)")]
    public HorrorActionSO actionDef;

    [Tooltip("Which GameObject to pass into that action")]
    public List<GameObject> targets;
}


[CreateAssetMenu(fileName = "HorrorEvent", menuName = "ScriptableObject/HorrorEvents/Event")]
public class HorrorEvent : ScriptableObject
{
    [Tooltip("Pairs of (action definition + target)")]
    public List<ActionBinding> bindings = new List<ActionBinding>();

    [Tooltip("If true, all actions will start at once; otherwise they run in sequence")]
    public bool runInParallel = false;
}
