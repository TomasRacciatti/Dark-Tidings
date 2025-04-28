using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Inventory;
using Inventory.Model;
using Items;
using UnityEngine;

public class ItemPrefab : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemObject itemObject;
    [SerializeField] private int Amount;
    
    private ItemAmount itemAmount;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        itemAmount.SetOverflow(true);
        itemAmount.SetItem(itemAmount);
    }

    private void OnEnable()
    {
        if (itemAmount.Item == null || itemAmount.Amount <= 0)
        {
            Destroy(gameObject);
            return;
        }
        //assign mesh and material
        if (itemAmount.Item.Mesh != null)
        {
            meshFilter.mesh = itemAmount.Item.Mesh;
        }
        if (itemAmount.Item.Materials != null && itemAmount.Item.Materials.Length > 0)
        {
            meshRenderer.materials = itemAmount.Item.Materials;
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
