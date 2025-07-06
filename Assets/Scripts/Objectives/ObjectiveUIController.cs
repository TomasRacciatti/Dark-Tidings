using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveField;

    private void OnEnable()
    {
        ObjectiveManager.Instance.OnObjectiveChanged += HandleObjectiveChanged;
        objectiveField.text = ObjectiveManager.Instance.CurrentObjective;
    }

    private void OnDisable()
    {
        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.OnObjectiveChanged -= HandleObjectiveChanged;
    }

    private void HandleObjectiveChanged(string newObjective)
    {
        objectiveField.text = newObjective;
    }
}