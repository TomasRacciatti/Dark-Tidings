using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public enum FlickerMode
    {
        ConstantRandom,
        TimedPattern
    }

    [SerializeField] private FlickerMode _mode = FlickerMode.ConstantRandom;

    [Header("Random Flicker Settings")] [SerializeField]
    private float _minFlickerInterval = 0.05f;
    [SerializeField] private float _maxFlickerInterval = 0.3f;

    [Header("Pattern Flicker Settings")] [SerializeField]
    private float[] _pattern = { 0.1f, 0.2f, 0.1f, 0.5f }; // on/off in sequence

    public bool IsLightOn { get; private set; } = true;

    private Light _light;
    private Coroutine _flickerRoutine;

    private void Awake()
    {
        _light = GetComponent<Light>();
        if (_light == null)
        {
            Debug.LogError($"[LightFlicker] No Light component found on {gameObject.name}.");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        _flickerRoutine = StartCoroutine(FlickerRoutine());
    }

    private void OnDisable()
    {
        if (_flickerRoutine != null)
            StopCoroutine(_flickerRoutine);
    }

    private IEnumerator FlickerRoutine()
    {
        switch (_mode)
        {
            case FlickerMode.ConstantRandom:
                while (true)
                {
                    IsLightOn = !IsLightOn;
                    float wait = Random.Range(_minFlickerInterval, _maxFlickerInterval);
                    yield return new WaitForSeconds(wait);
                }

            case FlickerMode.TimedPattern:
                int i = 0;
                while (true)
                {
                    IsLightOn = (i % 2 == 0); // even = on, odd = off
                    float wait = _pattern[i % _pattern.Length];
                    i++;
                    yield return new WaitForSeconds(wait);
                }

            default:
                yield break;
        }
    }
}