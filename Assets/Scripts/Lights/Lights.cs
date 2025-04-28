using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public abstract class Lights : MonoBehaviour
{
    [SerializeField] protected float _activeDistance = 20f;
    [SerializeField] protected Vector3 _detectionOffset = Vector3.zero;
    [SerializeField] protected LayerMask _playerLayerMask;
    [SerializeField] protected PlayerCharacter _player;

    protected Light _light;
    protected bool _isPlayerInRange;
    protected Coroutine _searchCoroutine;

    protected virtual void Awake()
    {
        _light = GetComponent<Light>();
        if (_light == null)
        {
            Debug.LogError($"[Lights] No Light component found on {gameObject.name}.");
            enabled = false;
        }
    }
    
    protected virtual void Start()
    {
        _light.enabled = false;
        _searchCoroutine = StartCoroutine(DetectPlayerRoutine());
    }

    protected virtual void Update()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position + _detectionOffset, _player.transform.position);
        _isPlayerInRange = distance <= _activeDistance;

        UpdateLightBehavior();
    }
    
    protected abstract void UpdateLightBehavior();
    
    private IEnumerator DetectPlayerRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);

        while (_player == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position + _detectionOffset, _activeDistance * 1.5f, _playerLayerMask);
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
    
    protected virtual void OnDisable()
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
