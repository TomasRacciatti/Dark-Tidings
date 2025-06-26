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
    
    [Header("Light State")]
    [SerializeField] private Mode _initialMode = Mode.On;

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

        SwitchMode(_initialMode);
    }

    protected override void UpdateLightBehavior()
    {
        // Aca quiero fadear, no apagar
        if (_mode == Mode.On)
        {
            if (_isPlayerInRange)
                _fadeBehavior.TurnOn();
            else
                _fadeBehavior.TurnOff();

            _fadeBehavior.UpdateBehavior();
            return;
        }
        
        // Apago si no esta en rango y no esta en state On
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