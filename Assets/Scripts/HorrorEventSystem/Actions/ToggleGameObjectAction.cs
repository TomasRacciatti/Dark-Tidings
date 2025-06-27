using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "ToggleGameObjectAction", menuName = "ScriptableObject/HorrorEvents/Actions/Toggle GameObject")]
public class ToggleGameObjectAction : HorrorActionSO
{
    public enum TargetMode { ByID, ByReference }
    
    public TargetMode mode = TargetMode.ByID;
    
    [Header("By ID Settings")]
    [SerializeField, Tooltip("ID of the scene object to show/hide (registered at Awake)")]
    private string objectId;
    
    [Header("General Settings")]
    [SerializeField, Tooltip("Should we activate (true) or deactivate (false) this object?")]
    private bool activate = true;
    [SerializeField, Tooltip("If true, pause the game and show cursor on activate; resume on deactivate")]
    private bool shouldPause = false;
    
    public override IEnumerator Execute()
    {
        if (mode != TargetMode.ByID)
        {
            Debug.LogWarning($"{nameof(ToggleGameObjectAction)} Execute() called in ByReference mode");
            yield break;
        }
        
        var targetObject = SceneObjectRegister.GetById(objectId);
        
        if (targetObject != null)
            targetObject.SetActive(activate);
        else
        {
            Debug.LogWarning($"ToggleGameObjectAction: no object registered with ID '{objectId}'");
            yield break;
        }
            
        if (shouldPause)
        {
            if (activate)
                PauseGame();
            else
                ResumeGame();
        }

        yield break;
    }

    public IEnumerator ExecuteOn(GameObject target) // Si no es por ID y es por ref viene para aca
    {
        if (mode != TargetMode.ByReference)
        {
            Debug.LogWarning($"{nameof(ToggleGameObjectAction)} ExecuteOn() called in ByID mode");
            yield break;
        }
        
        target.SetActive(activate);
        
        if (shouldPause)
        {
            if (activate)
                PauseGame();
            else
                ResumeGame();
        }
    }
    
    private void PauseGame()
    {
        GameManager.Pause(true);
    }

    private void ResumeGame()
    {
        GameManager.Pause(false);
    }
}
