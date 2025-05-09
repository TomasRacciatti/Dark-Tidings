using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using Inventory;
using Inventory.Controller;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [HideInInspector] public static CanvasManager Instance { get; private set; }

    [SerializeField] public PlayerController player;
    [SerializeField] public InventoryManager inventoryManager;
    [SerializeField] public TextMeshProUGUI LostUI;

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
