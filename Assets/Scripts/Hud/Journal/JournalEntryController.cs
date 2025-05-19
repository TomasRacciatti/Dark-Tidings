using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JournalEntryController : MonoBehaviour
{
    [SerializeField] ClueTypeData clueData;
    [SerializeField] TMP_Text displayNameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;
    
    int currentIndex = 0;

    void Awake()
    {
        leftArrow.onClick.AddListener(() => ChangeIndex(-1));
        rightArrow.onClick.AddListener(() => ChangeIndex(+1));
        RefreshUI();
    }

    void ChangeIndex(int delta)
    {
        var count = clueData.options.Count;
        currentIndex = (currentIndex + delta + count) % count;
        RefreshUI();
    }

    void RefreshUI()
    {
        var opt = clueData.options[currentIndex];
        displayNameText.text   = opt.displayName;
        descriptionText.text   = opt.descriptionText;
    }

    // Optional: expose a public Init(ClueTypeData) so you can assign data at runtime
    public void Init(ClueTypeData data)
    {
        clueData = data;
        currentIndex = 0;
        RefreshUI();
    }
}
