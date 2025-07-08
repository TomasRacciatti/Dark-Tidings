using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Play3DSoundAction", menuName = "ScriptableObject/HorrorEvents/Actions/Play Sound at Location")]
public class Play3DSoundAction : HorrorActionSO
{
    [System.Serializable]
    public struct ClipDef
    {
        public AudioClip clip;
        [Range(0,1)] public float volume;
    }
    
    [SerializeField] private List<ClipDef> clips = new List<ClipDef>();
    
    [SerializeField] private bool playInParallel = false;

    public IEnumerator ExecuteOn(AudioSource source)
    {
        if (source == null || clips.Count == 0)
            yield break;

        if (playInParallel)
        {
            float longest = 0f;

            foreach (var clipDef in clips)
            {
                source.PlayOneShot(clipDef.clip, clipDef.volume);
                longest = Mathf.Max(clipDef.clip.length, longest);
            }
            yield return new WaitForSeconds(longest);
        }
        else
        {
            foreach (var clipDef in clips)
            {
                source.PlayOneShot(clipDef.clip, clipDef.volume);
                yield return new WaitForSeconds(clipDef.clip.length);
            } 
        }
    }
    
    public override IEnumerator Execute()
    {
        throw new System.NotSupportedException(
            $"{nameof(Play3DSoundAction)} must be run via ExecuteOn(source)"
        );
    }
}
