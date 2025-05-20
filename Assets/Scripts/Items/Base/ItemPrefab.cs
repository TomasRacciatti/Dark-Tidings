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
            if (itemAmount.ItemInstance.SoItem.Mesh != null)
            {
                _meshFilter.mesh = itemAmount.ItemInstance.SoItem.Mesh;
            }
            if (itemAmount.ItemInstance.SoItem.Materials != null && itemAmount.ItemInstance.SoItem.Materials.Length > 0)
            {
                _meshRenderer.materials = itemAmount.ItemInstance.SoItem.Materials;
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
