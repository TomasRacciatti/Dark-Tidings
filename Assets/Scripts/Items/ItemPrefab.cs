using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Inventory;
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
        if (itemAmount.item == null || itemAmount.amount <= 0)
        {
            Destroy(gameObject);
        }
        //assign mesh and material
        if (itemAmount.item.mesh != null)
        {
            meshFilter.mesh = itemAmount.item.mesh;
        }
        if (itemAmount.item.materials != null && itemAmount.item.materials.Length > 0)
        {
            meshRenderer.materials = itemAmount.item.materials;
        }
    }

    public void Interact(GameObject interactableObject) //agarrar
    {
        if (interactableObject.TryGetComponent(out InventorySystem inventorySystem))
        {
            itemAmount.RemoveAmount(itemAmount.amount - inventorySystem.AddItem(itemAmount.item, itemAmount.amount));
            if (itemAmount.IsEmpty)
            {
                Destroy(gameObject);
            }
        }
    }
}
