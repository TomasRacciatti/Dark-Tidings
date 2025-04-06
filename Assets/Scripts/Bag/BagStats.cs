using UnityEngine;

[CreateAssetMenu(fileName = "New BackpackStats", menuName = "Inventory/BackpackStats")]
public class BackpackStats : ScriptableObject
{
    [Header("Debuff Stats")]
    [Tooltip("Speed reduction multiplier (0-1)")]
    [Range(0f, 1f)]
    public float SpeedMultiplier = 0.7f;
    
    [Tooltip("Extra damage taken multiplier (1+)")]
    [Range(1f, 2f)]
    public float DamageTakenMultiplier = 1.3f;
    
    [Tooltip("Accuracy reduction (increased spread cone in degrees)")]
    [Range(0f, 30f)]
    public float AccuracyReductionDegrees = 10f;
    
    [Header("Appearance")]
    public GameObject BackpackPrefab;
}