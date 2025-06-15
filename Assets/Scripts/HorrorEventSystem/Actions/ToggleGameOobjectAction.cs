using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameOobjectAction : HorrorActionSO
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
