using UnityEngine;
using Items.Tools;

namespace Objects.Clues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Clue/RadiationClue")]
    public class SO_RadiationClue : SO_Clue
    {
        [SerializeField] private int minRadiation;
        [SerializeField] private int maxRadiation;

        public (int, int) GetValue => (minRadiation, maxRadiation);
    }
}