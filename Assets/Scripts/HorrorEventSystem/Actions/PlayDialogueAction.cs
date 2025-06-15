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

    private AudioSource _source;
    
    public override IEnumerator Execute()
    {
        //Debug.Log("PlayDialogueAction.Execute() starting");
        
        // Si el indice no esta, volvemos de una
        var clip = dialogueBank?.GetClip(clipIndex);
        if (clip == null) yield break;

        if (_source == null)
        {
            var player = Object.FindObjectOfType<PlayerCharacter>(); // Ver de cambiar por algo menos costoso
            if (player == null)
                yield break;
        
            _source = player.GetComponentInChildren<AudioSource>();
            if (_source == null)
                yield break;
        }
        

        _source.PlayOneShot(clip, volume);
        
        yield return new WaitForSeconds(clip.length);
    }
}
