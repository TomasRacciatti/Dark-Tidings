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

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasFired && ((playerLayerMask.value & (1 << other.gameObject.layer)) != 0))
        {
            EventManager.Instance.Trigger(horrorEvent);
            hasFired = true;
        }
    }
}
