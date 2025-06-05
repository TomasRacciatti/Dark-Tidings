using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Clues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Clue/TempClue")]
    public class SO_ThermClue : SO_Clue
    {
        [SerializeField] private int minTemperature;
        [SerializeField] private int maxTemperature;

        public (int, int) GetValue => (minTemperature, maxTemperature);
    }
}