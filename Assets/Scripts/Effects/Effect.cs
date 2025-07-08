using System.Collections;
using UnityEngine;

namespace Effects
{
    public static class Effect
    {
        public static IEnumerator Fade(CanvasGroup canvasGroup, float from, float to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
                yield return null;
            }
            canvasGroup.alpha = to;
        }
    }
}
