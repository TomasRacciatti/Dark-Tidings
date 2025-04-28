using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : Lights
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
    
    private Coroutine _flickerRoutine;
    protected void OnEnable()
    {
        _flickerRoutine = StartCoroutine(FlickerRoutine());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_flickerRoutine != null)
        {
            StopCoroutine(_flickerRoutine);
            _flickerRoutine = null;
        }
    }
    
    protected override void UpdateLightBehavior()
    {
        _light.enabled = _isPlayerInRange && IsLightOn;
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