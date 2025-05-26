using System.Collections;
using System.Collections.Generic;
using Objects.Clues;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Journal/ClueData")]
public class ClueTypeData : ScriptableObject
{
    /*
    [System.Serializable]
    public struct ClueOption
    {
        public string displayName;
        [TextArea] public string descriptionText;
    }
    */

    //public List<ClueOption> options;

    public string clueTypeName;
    public SO_Clue[] clue;
}