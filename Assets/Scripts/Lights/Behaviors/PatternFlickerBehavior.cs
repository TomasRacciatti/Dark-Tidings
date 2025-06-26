using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternFlickerBehavior : ILightBehavior
{
    readonly MonoBehaviour _owner;
    readonly Light _light;
    readonly float[] _pattern;
    Coroutine _routine;
    bool _isOn = true;

    public PatternFlickerBehavior(MonoBehaviour owner, Light light, float[] pattern)
    {
        _owner   = owner;
        _light   = light;
        _pattern = pattern;
    }

    public void Enter()
    {
        if (_routine != null) _owner.StopCoroutine(_routine);
        _routine = _owner.StartCoroutine(Flicker());
    }

    public void Exit()
    {
        if (_routine != null)
        {
            _owner.StopCoroutine(_routine);
            _routine = null;
        }
        _light.enabled = true;
    }

    public void UpdateBehavior()
    {
        _light.enabled = _isOn;
    }

    IEnumerator Flicker()
    {
        int i = 0;
        while (true)
        {
            _isOn = (i % 2 == 0); // par y 0 = on, impar = off
            float wait = _pattern[i % _pattern.Length];
            i++;
            yield return new WaitForSeconds(wait);
        }
    }
}
