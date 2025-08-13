using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;

[CreateAssetMenu(fileName = "DoorStateAction", menuName = "ScriptableObject/HorrorEvents/Actions/Door Behavior")]
public class DoorStateAction : HorrorActionSO
{
    public Door.DoorMode mode = Door.DoorMode.Open;
    
    [Tooltip("Angle to use when opening (max ±120°)")]
    [Range(-120f, 120f)]
    public float targetAngle = 120f;
    
    [Tooltip("Spring force for opening/closing (higher = snappier/slammed)")]
    public float springForce = 100f;
    
    [Tooltip("If true, slam-close then apply supernatural lock")]
    public bool superLockAfterClose = false;
    
    public IEnumerator ExecuteOn(GameObject doorGO)
    {
        if (doorGO == null) yield break;
        var door = doorGO.GetComponent<Door>();
        if (door == null) yield break;

        // Always clear supernatural-lock when opening
        bool clearLock = (mode == Door.DoorMode.Open);

        door.SetDoorState(
            mode,
            targetAngle,
            springForce,
            clearLock,        // overrideLock: true if Opening, false otherwise
            superLockAfterClose
        );
        
        yield break;
    }

    public override IEnumerator Execute()
    {
        throw new System.NotSupportedException(
            $"{nameof(DoorStateAction)} must be run via ExecuteOn(targetGO)");
    }
}
