using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }
    public event Action<string> OnObjectiveChanged;
    
    [SerializeField] private string initialObjective = "Find what type of entity you are dealing with";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetObjective(string objective)
    {
        Debug.Log($"[ObjectiveManager] New objective: {objective}");
        OnObjectiveChanged?.Invoke(objective);
    }
}
