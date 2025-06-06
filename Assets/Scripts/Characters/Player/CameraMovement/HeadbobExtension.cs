using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadbobExtension : CinemachineExtension
{
    [SerializeField] private InputActionReference _moveAction = null;

    [Header("Bob Parameters")]
    [SerializeField] private float _verticalAmplitude = 0.02f;
    [SerializeField] private float _horizontalAmplitude = 0.01f;
    [SerializeField] private float _frequency = 8f;
    [SerializeField] private float _fadeSpeed = 10f;
    
    
    private float _currentVertBob   = 0f;
    private float _currentHorzBob   = 0f;
    private float _currentFrequency = 0f;

    private void OnEnable()
    {
        if (_moveAction != null)
            _moveAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_moveAction != null)
            _moveAction.action.Disable();
    }
    
    // This is called once per frame inside Cinemachine’s pipeline.
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Finalize)
            return;

        if (_moveAction == null)
            return;


        var moveInput = _moveAction.action.ReadValue<Vector2>();
        var speedFactor = moveInput.magnitude;
        
        var targetY   = speedFactor * _verticalAmplitude;
        var targetX   = speedFactor * _horizontalAmplitude;
        var targetFreq = speedFactor > 0.01f ? _frequency : 0f;
        
        _currentVertBob   = Mathf.Lerp(_currentVertBob,   targetY,   deltaTime * _fadeSpeed);
        _currentHorzBob   = Mathf.Lerp(_currentHorzBob,   targetX,   deltaTime * _fadeSpeed);
        _currentFrequency = Mathf.Lerp(_currentFrequency, targetFreq, deltaTime * _fadeSpeed);
        
        if (_currentFrequency < 0.001f)
            return;
        
        var t = Time.time * _currentFrequency;
        var bobY = Mathf.Sin(t) * _currentVertBob;
        var bobX = Mathf.Cos(t * 0.5f) * _currentHorzBob;
        
        var localOffset = new Vector3(bobX, bobY, 0f);
        var worldOffset = state.CorrectedOrientation * localOffset;
        
        state.PositionCorrection += worldOffset;
    }
}
