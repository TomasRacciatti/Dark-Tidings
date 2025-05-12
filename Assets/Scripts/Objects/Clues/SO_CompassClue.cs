using UnityEngine;
using Items.Tools;

namespace Objects.Clues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Clue/CompassClue")]
    public class SO_CompassClue : SO_Clue
    {
        [SerializeField] private CompassMode compassMode;
        public CompassMode CompassMode => compassMode;
    }
}