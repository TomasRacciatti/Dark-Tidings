using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Clues
{
    public class Clue : MonoBehaviour
    {
        [SerializeField] private SO_Clue _clueProvided;

        public SO_Clue GetClueProvided => _clueProvided;
    }
}