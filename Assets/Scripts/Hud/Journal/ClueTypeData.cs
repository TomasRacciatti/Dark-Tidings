using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Journal/ClueData")]
public class ClueTypeData : ScriptableObject
{
    [System.Serializable]
    public struct ClueOption
    {
        public string displayName;
        [TextArea] public string descriptionText;
    }

    public string clueTypeName;          
    public List<ClueOption> options;     
}
