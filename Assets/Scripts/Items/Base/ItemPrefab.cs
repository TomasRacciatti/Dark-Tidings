using System;
using Interfaces;
using Inventory.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Base
{
    public class ItemPrefab : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemAmount itemAmount;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
    
        [SerializeField] private Transform interactionPoint;
        public Transform InteractionPoint => interactionPoint != null ? interactionPoint : transform;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            if (itemAmount.IsEmpty)
            {
                Destroy(gameObject);
                return;
            }
            //assign mesh and material
            if (itemAmount.SoItem.Mesh != null)
            {
                _meshFilter.mesh = itemAmount.SoItem.Mesh;
            }
            if (itemAmount.SoItem.Materials != null && itemAmount.SoItem.Materials.Length > 0)
            {
                _meshRenderer.materials = itemAmount.SoItem.Materials;
            }
        }

        public void Interact(GameObject interactableObject) //agarrar
        {
            if (interactableObject.TryGetComponent(out InventorySystem inventorySystem))
            {
                itemAmount.RemoveAmount(itemAmount.Amount - inventorySystem.AddItem(itemAmount));
                if (itemAmount.IsEmpty)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
