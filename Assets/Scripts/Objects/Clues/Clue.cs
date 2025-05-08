using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class Clue : MonoBehaviour
{
    [SerializeField] private ClueType _clueProvided;
    [SerializeField] private float _clueRange = 10f;
    [SerializeField] private Vector3 _detectionOffset = Vector3.zero;

    public ClueType GetClueProvided => _clueProvided;
    public float GetClueRange => _clueRange;
    

    
    
    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _detectionOffset, _clueRange);

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