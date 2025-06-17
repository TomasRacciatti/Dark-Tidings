using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectRegister : MonoBehaviour
{
    // Static lookup table: ID → GameObject
    private static readonly Dictionary<string, GameObject> _registry = new();
    
    public static GameObject GetById(string id) => _registry.TryGetValue(id, out var go) ? go : null;
    
    [SerializeField, Tooltip("Unique ID for this scene object")]
    private string objectId;
    
    [SerializeField, Tooltip("The actual GameObject to register (can be inactive)")]
    private GameObject targetObject;

    private void Start()
    {
        if (string.IsNullOrEmpty(objectId) || targetObject == null)
        {
            Debug.LogWarning($"No ID on {name}");
            return;
        }
        _registry[objectId] = gameObject;
        Debug.Log($"[SceneObjectRegister] registered '{objectId}' for {gameObject.name}");
    }

    private void OnDestroy()
    {
        if (!string.IsNullOrEmpty(objectId))
            _registry.Remove(objectId);
    }
}
