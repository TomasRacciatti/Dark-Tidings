using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        Transform InteractionPoint { get; }
        public void Interact(GameObject interactableObject);
    }
}