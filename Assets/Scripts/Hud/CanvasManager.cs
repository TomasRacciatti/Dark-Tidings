using Inventory.Controller;
using TMPro;
using UnityEngine;

namespace Hud
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] public InventoryManager inventoryManager;
        [SerializeField] public TextMeshProUGUI LostUI;
        [SerializeField] public PausedMenu PauseMenu;
    }
}