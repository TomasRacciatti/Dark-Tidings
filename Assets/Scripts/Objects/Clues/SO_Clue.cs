using Items;
using UnityEngine;

namespace Objects.Clues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Clue/Default")]
    public class SO_Clue : ScriptableObject
    {
        [SerializeField] private SO_Item tool;
        public SO_Item Tool => tool;
    }
}