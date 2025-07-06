using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetObjective", menuName = "ScriptableObject/HorrorEvents/Actions/Update Objective")]
public class UpdateObjectiveAction : HorrorActionSO
{
    [Tooltip("The text to show in the objective panel")]
    public string objectiveText;

    public override IEnumerator Execute()
    {
        ObjectiveManager.Instance.SetObjective(objectiveText);
        yield break;
    }
}
