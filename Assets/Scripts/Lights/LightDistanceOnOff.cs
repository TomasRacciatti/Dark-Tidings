using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDistanceOnOff : MonoBehaviour
{
    // Setteo para que se apaguen a la distancia
    private Light _light;
    [SerializeField] private float _activeDistance = 20f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private LayerMask _playerLayerMask;
    
    private Coroutine _searchCoroutine;
    
    private bool _distanceAllowsLight = false;

    private void Awake()
    {
        _light = GetComponent<Light>();
        if (_light == null)
        {
            Debug.LogError($"[LightDistanceActivator] No Light component found on {gameObject.name}. This script requires a Light component on the same GameObject.");
            enabled = false;
        }
    }

    void Start()
    {
        _light.enabled = false;
    }
    
    void Update()
    {
        //DetectPlayerInRange();
        EnableLights();
        
        if (_player == null)
            _searchCoroutine = StartCoroutine(DetectPlayerRoutine());
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
        
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        _distanceAllowsLight = distance <= _activeDistance;
        ApplyFinalLightState();
    }

    private void ApplyFinalLightState()
    {
        bool finalState = _distanceAllowsLight;

        var flicker = GetComponent<LightFlicker>();
        if (flicker != null)
        {
            finalState &= flicker.IsLightOn;
        }

        _light.enabled = finalState;
    }

    private IEnumerator DetectPlayerRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);

        while (_player == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _activeDistance * 1.5f, _playerLayerMask);

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

    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta; 
        Gizmos.DrawWireSphere(transform.position, _activeDistance); 
    }
}
