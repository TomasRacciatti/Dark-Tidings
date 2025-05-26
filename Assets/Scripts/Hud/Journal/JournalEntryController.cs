using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        clueTypeText.text = clueData.clueTypeName;
        leftArrow.onClick.AddListener(() => ChangeIndex(-1));
        rightArrow.onClick.AddListener(() => ChangeIndex(+1));
        RefreshUI();
    }

    void ChangeIndex(int delta)
    {
        var count = clueData.clue.Length;
        currentIndex = (currentIndex + delta + count) % count;
        RefreshUI();
    }

    void RefreshUI()
    {
        var opt = clueData.clue[currentIndex];
        valueText.text   = opt.Value;
        resultText.text   = opt.Result;
    }
}
