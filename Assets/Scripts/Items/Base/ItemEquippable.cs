using UnityEngine;

namespace Items.Base
{
    public abstract class ItemEquippable : MonoBehaviour
    {
        [SerializeField] public SO_Item soItem;
        public abstract void Use(UseType useType);
        
        [HideInInspector] public ItemAmount itemAmount;
    }

    public enum UseType
    {
        Default,
        Aim,
        Reload1,
        Reload2,
        Reload3,
    }
}