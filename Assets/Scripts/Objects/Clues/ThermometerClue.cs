using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Clue/TempClue")]
public class ThermometerClue : ClueType
{
    [SerializeField] private int _minTemperature;
    [SerializeField] private int _maxTemperature;

    public float GetTemperature()
    {
        return Random.Range((float)_minTemperature, _maxTemperature);
    }
}
