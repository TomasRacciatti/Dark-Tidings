using Characters.Player;
using Interfaces;
using UnityEngine;

namespace Inventory.Model
{
    public class BoltInventory : FiniteInventory, IInteractable
    {
        [SerializeField] private Transform interactionPoint;
        public Transform InteractionPoint => interactionPoint != null ? interactionPoint : transform;
        
        public void Interact(GameObject interactableObject)
        {
            if (interactableObject.TryGetComponent(out PlayerCharacter playerCharacter))
            {
                
            }
        }
    }
}