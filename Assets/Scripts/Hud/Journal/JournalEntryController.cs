using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JournalEntryController : MonoBehaviour
{
    [SerializeField] ClueTypeData clueData;
    [SerializeField] TMP_Text clueTypeText;
    [SerializeField] TMP_Text valueText;
    [SerializeField] TMP_Text resultText;
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;
    
    int currentIndex = 0;
    
    [SerializeField] private AudioCue audioCue;
    [SerializeField] private AudioSource _source;
    
    public string CurrentValue => clueData.clue[currentIndex].Value;
    
    void Awake()
    {
        clueTypeText.text = clueData.clueTypeName;
        leftArrow.onClick.AddListener(() => ChangeIndex(-1));
        rightArrow.onClick.AddListener(() => ChangeIndex(+1));
        RefreshUI();
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);
        _source = GameManager.Player.GetComponentInChildren<AudioSource>();
    }

    void ChangeIndex(int delta)
    {
        var count = clueData.clue.Length;
        currentIndex = (currentIndex + delta + count) % count;
        PlayWritingClip();
        RefreshUI();
    }

    void RefreshUI()
    {
        var opt = clueData.clue[currentIndex];
        valueText.text   = opt.Value;
        resultText.text   = opt.Result;
    }
    
    public void PlayWritingClip()
    {
        var clip = audioCue.GetRandomClip();
        if (clip != null)
        {
            _source.PlayOneShot(clip);
        }
    }
}
