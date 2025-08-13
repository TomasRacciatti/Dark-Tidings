using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup objectivePanel;
    [SerializeField] private TMP_Text objectiveField;
    
    [SerializeField] private float fadeDuration = 0.2f;
    
    private Coroutine _currentFade;

    private void OnEnable()
    {
        ObjectiveManager.Instance.OnObjectiveChanged += HandleObjectiveChanged;
        objectivePanel.alpha = 1f;
        objectiveField.text = ObjectiveManager.Instance.CurrentObjective;
    }

    private void OnDisable()
    {
        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.OnObjectiveChanged -= HandleObjectiveChanged;
    }

    private void HandleObjectiveChanged(string newObjective)
    {
        if (_currentFade != null)
            StopCoroutine(_currentFade);

        _currentFade = StartCoroutine(FadeAndSwap(newObjective));
    }

    private IEnumerator FadeAndSwap(string newText)
    {
        yield return Fade(1f, 0f);
        objectiveField.text = newText;
        yield return Fade(0f, 1f);
        _currentFade = null;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            objectivePanel.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        objectivePanel.alpha = to;
    }
}