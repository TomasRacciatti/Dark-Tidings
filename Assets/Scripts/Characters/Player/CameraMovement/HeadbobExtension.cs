using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class HeadBobExtension : CinemachineExtension
{
    [SerializeField] private InputActionReference _moveAction = null;
    [SerializeField] private InputActionReference _sprintAction = null;

    [Header("Bob Parameters")]
    [SerializeField] private float _baseVerticalAmplitude = 0.02f;
    [SerializeField] private float _baseHorizontalAmplitude = 0.01f;
    [SerializeField] private float _baseFrequency = 6f;
    [SerializeField] private float _sprintMultiplier = 2f;
    [SerializeField] private float _smooth = 10f;


    private float _bobTimer = 0f;
    private bool _wasMoving = false;
    private Vector2 _actualOffset2D = Vector2.zero;

    private void OnEnable()
    {
        if (_moveAction != null) _moveAction.action.Enable();
        if (_sprintAction != null) _sprintAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_moveAction != null) _moveAction.action.Disable();
        if (_sprintAction != null) _sprintAction.action.Disable();
    }
    
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Finalize)
            return;

        if (_moveAction == null)
            return;
        
        var moveInput = _moveAction.action.ReadValue<Vector2>();
        var isSprinting = (_sprintAction != null && _sprintAction.action.ReadValue<float>() > 0.5f);
        
        var speedFactor = moveInput.magnitude;


        var isMovingNow = speedFactor > 0.01f;
        if (isMovingNow && !_wasMoving)
        {
            _bobTimer = 0f;
        }
        _wasMoving = isMovingNow;

        var freqMultiplier = isSprinting ? _sprintMultiplier : 1;
        
        if (isMovingNow)
        {
            _bobTimer += deltaTime * (_baseFrequency * freqMultiplier);
        }
        
        var vertAmp = _baseVerticalAmplitude * speedFactor * (isSprinting ? _sprintMultiplier : 1);
        var horzAmp = _baseHorizontalAmplitude * speedFactor * (isSprinting ? _sprintMultiplier : 1);
        
        var desiredVert = Mathf.Sin(_bobTimer) * vertAmp;
        var desiredHorz = Mathf.Sin(_bobTimer * 0.5f) * horzAmp;
        var desiredOffset2D = new Vector2(desiredHorz, desiredVert);


        _actualOffset2D = Vector2.Lerp(
            _actualOffset2D,
            desiredOffset2D,
            deltaTime * _smooth
        );
        
        if (_actualOffset2D.sqrMagnitude < 0.000001f)
            return;

        var localOffset3D = new Vector3(_actualOffset2D.x, _actualOffset2D.y, 0f);
        var worldOffset = state.CorrectedOrientation * localOffset3D;
        
        state.PositionCorrection += worldOffset;
    }
}
