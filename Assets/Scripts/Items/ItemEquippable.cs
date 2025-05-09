using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public abstract class ItemEquippable : MonoBehaviour
    {
        [SerializeField] public SO_Item soItem;
        public abstract void Use();
    }
}