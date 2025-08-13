using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VolumeTrigger : HorrorEventTrigger
{

    [SerializeField] private LayerMask playerLayerMask = 1 << 9;
    
    private Color gizmoColor = new Color(0f, 1f, 0f, 0.1f);

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayerMask.value & (1 << other.gameObject.layer)) != 0)
            Fire();
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
