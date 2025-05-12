using UnityEngine;

namespace Objects.Clues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Clue/TempClue")]
    public class SO_ThermClue : SO_Clue
    {
        [SerializeField] private int _minTemperature;
        [SerializeField] private int _maxTemperature;

        public float GetTemperature()
        {
            return Random.Range((float)_minTemperature, _maxTemperature);
        }
    }
}