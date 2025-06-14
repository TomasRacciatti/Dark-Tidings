using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

[CreateAssetMenu(fileName = "ShowDialogueAction", menuName = "ScriptableObject/HorrorEvents/Actions/Play Dialogue")]
public class PlayDialogueAction : HorrorActionSO
{
    [SerializeField] private DialogueBankSO  dialogueBank;

    [SerializeField] private int clipIndex;
    
    [SerializeField] private float volume = 1f;
    
    public override IEnumerator Execute()
    {
        // Si el indice no esta, volvemos de una
        var clip = dialogueBank?.GetClip(clipIndex);
        if (clip == null) yield break;

        var player = Object.FindObjectOfType<PlayerCharacter>(); // Ver de cambiar por algo menos costoso
        if (player == null)
            yield break;
        
        var source = player.GetComponent<AudioSource>();
        if (source == null)
            yield break;

        source.PlayOneShot(clip, volume);
        
        yield return new WaitForSeconds(clip.length);
    }
}
