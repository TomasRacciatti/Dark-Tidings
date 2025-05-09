using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;


[CreateAssetMenu(menuName = "ScriptableObject/Clue/Default")]
public class ClueType : ScriptableObject
{
    [SerializeField] private SO_Item tool;
}
