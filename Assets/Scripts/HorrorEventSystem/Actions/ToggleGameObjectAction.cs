using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToggleGameObjectAction", menuName = "ScriptableObject/HorrorEvents/Actions/Toggle GameObject")]
public class ToggleGameObjectAction : HorrorActionSO
{
    [SerializeField, Tooltip("ID of the scene object to show/hide (registered at Awake)")]
    private string objectId;
    
    [SerializeField, Tooltip("Should we activate (true) or deactivate (false) this object?")]
    private bool activate = true;
    
    public override IEnumerator Execute()
    {
        Debug.Log($"[ToggleAction] Firing for ID='{objectId}'");
        
        var targetObject = SceneObjectRegister.GetById(objectId);
        
        Debug.Log(targetObject != null
            ? $"[ToggleAction] Found GameObject '{targetObject.name}', active={targetObject.activeSelf}"
            : $"[ToggleAction] Couldn’t find any object registered under '{objectId}'");

        if (targetObject != null)
        {
            targetObject.SetActive(activate);
            Debug.Log($"[ToggleAction] Now active={targetObject.activeSelf}");
        }
        else
            Debug.LogWarning($"ToggleGameObjectAction: no object registered with ID '{objectId}'");
        
        yield break;
    }
}
