using UnityEngine;

namespace Items
{
    public abstract class ItemEquippable : MonoBehaviour
    {
        [SerializeField] public ItemObject itemObject;
        public abstract void Interact();
    }
}