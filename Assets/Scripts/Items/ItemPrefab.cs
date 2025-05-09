using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Inventory;
using Inventory.Model;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemPrefab : MonoBehaviour, IInteractable
{
    [SerializeField] private SO_Item soItem;
    [SerializeField] private int Amount;
    
    private ItemAmount itemAmount;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
    [SerializeField] private Transform interactionPoint;
    public Transform InteractionPoint => interactionPoint != null ? interactionPoint : transform;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        itemAmount = new ItemAmount(soItem, Amount, true);
    }

    private void OnEnable()
    {
        if (itemAmount.ItemInstance == null || itemAmount.Amount <= 0)
        {
            Destroy(gameObject);
            return;
        }
        //assign mesh and material
        if (itemAmount.ItemInstance.Mesh != null)
        {
            meshFilter.mesh = itemAmount.ItemInstance.Mesh;
        }
        if (itemAmount.ItemInstance.Materials != null && itemAmount.ItemInstance.Materials.Length > 0)
        {
            meshRenderer.materials = itemAmount.ItemInstance.Materials;
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
