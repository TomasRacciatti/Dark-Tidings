using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : Lights
{
    public enum Mode
    {
        Off,
        On,
        FlickerRandom,
        FlickerPattern
    }

    [Header("Fade Settings")] [SerializeField]
    private float _fadeSpeed = 2f;

    [Header("Random Flicker Settings")] [SerializeField]
    private float _minFlickerInterval = 0.05f;

    [SerializeField] private float _maxFlickerInterval = 0.3f;

    [Header("Pattern Flicker Settings")] [SerializeField]
    private float[] _pattern = { 0.5f, 0.5f };

    private FadeBehavior _fadeBehavior;
    private RandomFlickerBehavior _randomFlicker;
    private PatternFlickerBehavior _patternFlicker;
    private ILightBehavior _currentBehavior;
    private Mode _mode = Mode.Off;


    protected override void Awake()
    {
        base.Awake();

        _fadeBehavior = new FadeBehavior(_light, _fadeSpeed);
        _randomFlicker = new RandomFlickerBehavior(this, _light, _minFlickerInterval, _maxFlickerInterval);
        _patternFlicker = new PatternFlickerBehavior(this, _light, _pattern);
    }

    protected override void Start()
    {
        base.Start();

        SwitchMode(Mode.Off);
    }

    protected override void UpdateLightBehavior()
    {
        // Apago si no esta en rango
        if (!_isPlayerInRange)
        {
            _currentBehavior?.Exit();
            _currentBehavior = null;
            _light.enabled = false;
            return;
        }

        _currentBehavior?.UpdateBehavior();
    }

    public void SwitchMode(Mode newMode) // Podemos llamar esto en el event system para cambiar el modo?
    {
        _currentBehavior?.Exit(); // Limpiamos el behavior anterior

        _mode = newMode;
        switch (_mode)
        {
            case Mode.Off:
                _currentBehavior = null;
                _light.enabled = false;
                break;

            case Mode.On:
                _currentBehavior = _fadeBehavior;
                _fadeBehavior.Enter();
                break;

            case Mode.FlickerRandom:
                _currentBehavior = _randomFlicker;
                _randomFlicker.Enter();
                break;

            case Mode.FlickerPattern:
                _currentBehavior = _patternFlicker;
                _patternFlicker.Enter();
                break;
        }
    }
}