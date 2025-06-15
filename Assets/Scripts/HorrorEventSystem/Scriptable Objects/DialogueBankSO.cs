using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBankSO", menuName = "ScriptableObject/HorrorEvents/Dialogue Bank")]
public class DialogueBankSO : ScriptableObject
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    
    public AudioClip GetClip(int index)
    {
        return (index >= 0 && index < audioClips.Count) ? audioClips[index] : null;
    }
    
    public int Count => audioClips.Count;
}
