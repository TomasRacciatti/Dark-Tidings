using UnityEngine;

namespace Items.Base
{
    public abstract class ItemEquippable : MonoBehaviour
    {
        [SerializeField] public SO_Item soItem;
        public abstract void Use();
        
        
    }
}