using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportAction : HorrorActionSO
{
    [Tooltip("The Transform whose position (and optionally rotation) we copy")]
    [SerializeField] private Transform destination;

    [Tooltip("Also match destination.rotation?")]
    [SerializeField] private bool matchRotation = true;
    
    
    public IEnumerator ExecuteOn(GameObject target)
    {
        if (target == null || destination == null)
            yield break;

        var t = target.transform;
        t.position = destination.position;
        if (matchRotation)
            t.rotation = destination.rotation;

        yield break;
    }

    public override IEnumerator Execute()
    {
        throw new System.NotSupportedException(
            $"{nameof(TransportAction)} must be run via ExecuteOn(target)");
    }
}
