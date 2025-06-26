using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class LightFade : Lights
{
    [SerializeField] private float _fadeSpeed = 2f;
    private float _targetIntensity;
    private float _originalIntensity;

    protected override void Awake()
    {
        base.Awake();
        _originalIntensity = _light.intensity;
    }

    protected override void Start()
    {
        base.Start();
        _light.intensity = 0f;
    }

    protected override void UpdateLightBehavior()
    {
        _targetIntensity = _isPlayerInRange ? _originalIntensity : 0f;
        UpdateLight();
    }

    public void UpdateLight()
    {
        if (_targetIntensity > 0f && !_light.enabled)
            _light.enabled = true;

        _light.intensity = Mathf.MoveTowards(_light.intensity, _targetIntensity, _fadeSpeed * Time.deltaTime);

        if (_light.intensity <= 0.01f && _targetIntensity == 0f)
            _light.enabled = false;
    }
}