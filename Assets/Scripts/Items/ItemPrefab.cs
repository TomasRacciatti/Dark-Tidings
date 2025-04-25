using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Inventory;
using Inventory.Model;
using UnityEngine;

public class ItemPrefab : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemAmount itemAmount;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        if (itemAmount.Item == null || itemAmount.Amount <= 0)
        {
            Destroy(gameObject);
            return;
        }
        //assign mesh and material
        if (itemAmount.Item.GetMesh() != null)
        {
            meshFilter.mesh = itemAmount.Item.GetMesh();
        }
        if (itemAmount.Item.GetMaterials() != null && itemAmount.Item.GetMaterials().Length > 0)
        {
            meshRenderer.materials = itemAmount.Item.GetMaterials();
        }
    }

    public void Interact(GameObject interactableObject) //agarrar
    {
        if (interactableObject.TryGetComponent(out InventorySystem inventorySystem))
        {
            itemAmount.RemoveAmount(itemAmount.Amount - inventorySystem.AddItem(itemAmount.Item, itemAmount.Amount));
            if (itemAmount.IsEmpty)
            {
                Destroy(gameObject);
            }
        }
    }
}
