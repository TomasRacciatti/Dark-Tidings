using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Clues
{
    public class Clue : MonoBehaviour
    {
        [SerializeField] private SO_Clue _clueProvided;

        public SO_Clue GetClueProvided => _clueProvided;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnTriggerExit(Collider other)
        {
            
        }
    }
}