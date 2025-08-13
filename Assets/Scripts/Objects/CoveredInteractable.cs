using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class CoveredInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform interactionPoint;
    public Transform InteractionPoint => interactionPoint != null ? interactionPoint : transform;
    public void Interact(GameObject interactableObject)
    {
        Destroy(gameObject);
        // Aca poner la animacion de la sabana cayendo
    }
}
