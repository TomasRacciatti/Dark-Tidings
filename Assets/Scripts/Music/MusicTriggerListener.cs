using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerListener : MonoBehaviour
{
    [System.Serializable]
    public class Entry
    {
        public HorrorEventTrigger trigger;
        public AudioClip clip;
        public bool loop = true;
        public float fadeTime = 1f;
    }
    
    [Tooltip("Any trigger here will switch the music")]
    [SerializeField]
    private List<Entry> entries = new List<Entry>();
    
    private void OnEnable()
    {
        foreach (var e in entries)
            if (e.trigger != null)
                e.trigger.FiredEvent += () => MusicManager.Instance.PlayMusic(e.clip, e.loop, e.fadeTime);
    }
    
    private void OnDisable()
    {
        foreach (var e in entries)
            if (e.trigger != null)
                e.trigger.FiredEvent -= () => MusicManager.Instance.PlayMusic(e.clip, e.loop, e.fadeTime);
    }
}