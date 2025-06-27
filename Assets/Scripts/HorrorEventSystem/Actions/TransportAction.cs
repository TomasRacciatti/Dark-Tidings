using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransportAction", menuName = "ScriptableObject/HorrorEvents/Actions/Transport Game Object")]
 class TransportAction : HorrorActionSO
{

    [Tooltip("Also match destination.rotation")]
    [SerializeField] private bool matchRotation = true;
    
    
    public IEnumerator ExecuteOn(GameObject target, GameObject destination)
    {
        if (target == null || destination == null)
            yield break;

        var t = target.transform;
        t.position = destination.transform.position;
        if (matchRotation)
            t.rotation = destination.transform.rotation;

        yield break;
    }

    public override IEnumerator Execute()
    {
        throw new System.NotSupportedException(
            $"{nameof(TransportAction)} must be run via ExecuteOn(target)");
    }
}
