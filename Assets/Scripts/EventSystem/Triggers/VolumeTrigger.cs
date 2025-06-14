using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VolumeTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("Drag in the HorrorEvents you want to fire here")]
    private HorrorEvent horrorEvent;

    [SerializeField] private LayerMask playerLayerMask = 1 << 9;
    
    private bool hasFired = false;
    
    private Color gizmoColor = new Color(0f, 1f, 0f, 0.1f);

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasFired && ((playerLayerMask.value & (1 << other.gameObject.layer)) != 0))
        {
            //Debug.Log($"VolumeTrigger hit by {other.name} (layer {other.gameObject.layer})");
            
            EventManager.Instance.Trigger(horrorEvent);
            hasFired = true;
        }
    }
    
    void OnDrawGizmos()
    {
        var box = GetComponent<BoxCollider>();
        if (box == null) return;

        Gizmos.color = gizmoColor;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(box.center, box.size);
    }
}
