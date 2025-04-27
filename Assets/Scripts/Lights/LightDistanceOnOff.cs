using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class LightDistanceOnOff : MonoBehaviour
{
    // Setteo para que se apaguen a la distancia
    private Light _light;
    private LightFlicker _flicker;

    [SerializeField] private float _activeDistance = 20f;
    [SerializeField] private Vector3 _detectionOffset = Vector3.zero;

    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private LayerMask _playerLayerMask;

    private Coroutine _searchCoroutine;

    private bool _distanceAllowsLight = false;

    // Smooth Blend
    [SerializeField] private float _fadeSpeed = 2f;
    private float _targetIntensity;
    private float _originalIntensity;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _flicker = GetComponent<LightFlicker>();

        if (_light == null)
        {
            Debug.LogError(
                $"[LightDistanceActivator] No Light component found on {gameObject.name}. This script requires a Light component on the same GameObject.");
            enabled = false;
        }

        _originalIntensity = _light.intensity;
    }

    void Start()
    {
        _light.enabled = false;
        _light.intensity = 0f;
    }

    void Update()
    {
        //DetectPlayerInRange();

        if (_player == null)
            _searchCoroutine = StartCoroutine(DetectPlayerRoutine());

        EnableLights();
        UpdateLightState();
    }


    /*
    private void DetectPlayerInRange()
    {
        if (_player != null)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, _activeDistance * 1.5f, _playerLayerMask);

        foreach (var hit in hits)
        {
            PlayerCharacter candidate = hit.GetComponent<PlayerCharacter>();
            if (candidate != null)
            {
                _player = candidate;
                break;
            }
        }
    }
    */

    private void EnableLights()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position + _detectionOffset, _player.transform.position);
        _distanceAllowsLight = distance <= _activeDistance;
    }

    private void UpdateLightState()
    {
        if (_flicker != null)
        {
            ApplyFlickerLightState();
        }
        else
        {
            ApplyFinalLightState();
            ApplyFadeLogic();
        }
    }

    private void ApplyFinalLightState()
    {
        bool finalState = _distanceAllowsLight;
        _targetIntensity = finalState ? _originalIntensity : 0f;
    }

    private void ApplyFlickerLightState()
    {
        bool finalState = _distanceAllowsLight & _flicker.IsLightOn;
        _light.enabled = finalState;
    }

    private void ApplyFadeLogic()
    {
        if (_targetIntensity > 0f && !_light.enabled)
        {
            _light.enabled = true;
        }

        _light.intensity = Mathf.MoveTowards(_light.intensity, _targetIntensity, _fadeSpeed * Time.deltaTime);

        if (_light.intensity <= 0.01f && _targetIntensity == 0f)
        {
            _light.enabled = false;
        }
    }

    private IEnumerator DetectPlayerRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);

        while (_player == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position + _detectionOffset, _activeDistance * 1.5f,
                _playerLayerMask);

            foreach (var hit in hits)
            {
                PlayerCharacter candidate = hit.GetComponent<PlayerCharacter>();
                if (candidate != null)
                {
                    _player = candidate;
                    yield break;
                }
            }

            yield return wait;
        }
    }

    private void OnDisable()
    {
        if (_searchCoroutine != null)
            StopCoroutine(_searchCoroutine);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + _detectionOffset, _activeDistance);

#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawLine(transform.position, transform.position + _detectionOffset);
        UnityEditor.Handles.SphereHandleCap(
            0,
            transform.position + _detectionOffset,
            Quaternion.identity,
            0.2f,
            EventType.Repaint
        );
#endif
    }
}