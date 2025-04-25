using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Controller;
using UnityEngine;

public class CanvasGameManager : MonoBehaviour
{
    [HideInInspector] public static CanvasGameManager Instance { get; private set; }

    [SerializeField] public InventoryManager inventoryManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Previene duplicados
            return;
        }
        Instance = this;
    }
}
