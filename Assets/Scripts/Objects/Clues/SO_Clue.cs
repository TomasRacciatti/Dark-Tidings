using Items;
using Items.Base;
using UnityEngine;

namespace Objects.Clues
{
    public class SO_Clue : ScriptableObject
    {
        [SerializeField] private SO_Item tool;
        [SerializeField] private string value;
        [SerializeField , TextArea] private string result;
        public SO_Item Tool => tool;
        public string Value => value;
        public string Result => result;
    }
}