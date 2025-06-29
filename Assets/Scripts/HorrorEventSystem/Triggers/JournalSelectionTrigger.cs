using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalSelectionTrigger : HorrorEventTrigger
{
    [Header("Journal Selection")]
    [Tooltip("Desired value we want to select")] [SerializeField]
    private string targetValue = "Negative";
    [SerializeField] private float holdDuration = 1.0f;
    [SerializeField] private JournalEntryController _journalEntryController;

    private void OnEnable()
    {
        StartCoroutine(HoldCheck());
    }

    private IEnumerator HoldCheck()
    {
        while (true)
        {
            if (hasFired && fireOnce)
                yield break;
            
            bool match = _journalEntryController.CurrentValue == targetValue;

            if (match)
            {
                float t = 0f;
                while (t < holdDuration)
                {
                    if (_journalEntryController.CurrentValue != targetValue)
                        break;

                    t += Time.deltaTime;
                    yield return null;
                }
                
                if (t >= holdDuration)
                {
                    Fire();
                    yield break;
                }
            }

            yield return null;
        }
    }
}