using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DamagedPlayer : MonoBehaviour
{
    /*public static DamagedPlayer instance;
    private CustomPassVolume customPassVolume;

    private void Awake()
    {
        instance = this;
        customPassVolume = GetComponent<CustomPassVolume>();
        Deactivate();
    }

    public static void Damaged()
    {
        instance.customPassVolume.customPasses[0].enabled = true;
        instance.CancelInvoke(nameof(Deactivate));
        instance.Invoke(nameof(Deactivate), 1);
    }

    private void Deactivate()
    {
        customPassVolume.customPasses[0].enabled = false;
    }*/

    public Material screenDamageMat;
    private Coroutine screenDamageTask;

    public static DamagedPlayer instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        screenDamageMat.SetFloat("_Intensity", 0);
    }

    public void ScreenDamageEffect(float intensity)
    {
        if (screenDamageTask != null)
            StopCoroutine(screenDamageTask);

        screenDamageTask = StartCoroutine(screenDamage(intensity));
    }

    private IEnumerator screenDamage(float targetIntensity)
    {
        float currentIntensity = screenDamageMat.GetFloat("_Intensity");
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration); // t va de 0 a 1
            float interpolated = Mathf.Lerp(currentIntensity, targetIntensity, 4 * t);
            screenDamageMat.SetFloat("_Intensity", interpolated);
            print(interpolated);
            yield return null;
        }
        
        screenDamageMat.SetFloat("_Intensity", targetIntensity);
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }
}