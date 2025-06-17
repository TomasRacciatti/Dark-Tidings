using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToggleGameObjectAction", menuName = "ScriptableObject/HorrorEvents/Actions/Toggle GameObject")]
public class ToggleGameObjectAction : HorrorActionSO
{
    [SerializeField] private GameObject targetObject;
    
    [SerializeField, Tooltip("Should we activate (true) or deactivate (false) this object?")]
    private bool activate = true;
    
    public override IEnumerator Execute()
    {
        if (targetObject != null)
            targetObject.SetActive(activate);
        yield break;
    }
}
