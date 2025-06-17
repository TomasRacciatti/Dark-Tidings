using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "ToggleGameObjectAction", menuName = "ScriptableObject/HorrorEvents/Actions/Toggle GameObject")]
public class ToggleGameObjectAction : HorrorActionSO
{
    [SerializeField, Tooltip("ID of the scene object to show/hide (registered at Awake)")]
    private string objectId;
    
    [SerializeField, Tooltip("Should we activate (true) or deactivate (false) this object?")]
    private bool activate = true;
    
    [SerializeField, Tooltip("If true, pause the game and show cursor on activate; resume on deactivate")]
    private bool shouldPause = false;
    
    public override IEnumerator Execute()
    {
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
            if (activate)   PauseGame();
            else            ResumeGame();
        }

        yield break;
    }
    
    private void PauseGame()
    {
        GameManager.Pause(true);

        // disable camera input
        
    }

    private void ResumeGame()
    {
        GameManager.Pause(false);

        // Enable Input
        
    }
}
