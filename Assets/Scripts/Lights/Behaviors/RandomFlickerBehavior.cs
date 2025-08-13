using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlickerBehavior : ILightBehavior
{
    readonly MonoBehaviour _owner;
    readonly Light _light;
    readonly float _minInterval, _maxInterval;
    Coroutine _routine;
    bool _isOn = true;

    public RandomFlickerBehavior(MonoBehaviour owner, Light light, float minInterval, float maxInterval)
    {
        _owner        = owner;
        _light        = light;
        _minInterval  = minInterval;
        _maxInterval  = maxInterval;
    }
    
    public void Enter()
    {
        // Tenemos que asegurarnos que la corrutina anterior se haya frenado
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
        // por default la estamos dejando prendida cuando exitea
        _light.enabled = true;
    }

    public void UpdateBehavior()
    {
        _light.enabled = _isOn;
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            _isOn = !_isOn;
            yield return new WaitForSeconds(
                Random.Range(_minInterval, _maxInterval)
            );
        }
    }
}
