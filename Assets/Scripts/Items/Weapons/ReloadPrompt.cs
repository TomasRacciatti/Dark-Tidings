using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPrompt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float scaleAmount = 0.5f;
    [SerializeField] private float speed = 2f;

    private Vector3 _originalScale;

    void Start()
    {
        if (target == null) target = transform;
        _originalScale = target.localScale;
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime * speed;
            float scale = Mathf.Lerp(1f, scaleAmount, (Mathf.Sin(timer) + 1f) / 2f);
            target.localScale = _originalScale * scale;
            yield return null;
        }
    }
}
