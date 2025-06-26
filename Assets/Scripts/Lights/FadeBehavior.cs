using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehavior : ILightBehavior
{
    readonly Light _light;
    readonly float _fadeSpeed;
    readonly float _originalIntensity;
    float _targetIntensity;

    public FadeBehavior(Light light, float fadeSpeed)
    {
        _light = light;
        _fadeSpeed = fadeSpeed;
        _originalIntensity = light.intensity;
    }

    public void Enter()
    {
        _light.enabled = true;
        _targetIntensity = _originalIntensity;
    }

    public void Exit()
    {
        
    }

    public void UpdateBehavior()
    {
        _light.intensity = Mathf.MoveTowards(
            _light.intensity,
            _targetIntensity,
            _fadeSpeed * Time.deltaTime
        );
    }
    
    public void TurnOn()
    {
        _targetIntensity = _originalIntensity;
        if (!_light.enabled)
            _light.enabled = true;
    }
    
    public void TurnOff()
    {
        _targetIntensity = 0f;
        if (_light.intensity <= 0.01f)
            _light.enabled = false;
    }
}
