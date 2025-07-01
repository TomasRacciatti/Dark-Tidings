using System.Collections;
using System.Collections.Generic;
using Characters.Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "ActivateEnemyAction", menuName = "ScriptableObject/HorrorEvents/Actions/Activate Enemy")]
public class ActivateEnemyAction : HorrorActionSO
{
    [SerializeField, Tooltip("If true, sets isActive = true; if false, sets isActive = false")]
    private bool activate = true;
    
    public IEnumerator ExecuteOn(GameObject target)
    {
        if (target == null)
            yield break;

        var enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.SetActive(activate);
        }
        else
        {
            Debug.LogWarning($"SetActiveActionSO: '{target.name}' has no EnemyController");
        }

        yield break;
    }

    public override IEnumerator Execute()
    {
        throw new System.NotSupportedException(
            $"{nameof(ActivateEnemyAction)} must be run via ExecuteOn(targetGO)");
    }
}
