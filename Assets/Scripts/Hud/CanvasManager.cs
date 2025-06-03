using Inventory.Controller;
using Items.Base;
using Managers;
using TMPro;
using UnityEngine;

namespace Hud
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] public InventoryManager inventoryManager;
        [SerializeField] public TextMeshProUGUI LostUI;
        [SerializeField] public PausedMenu PauseMenu;
        
        public void InventoryUI(int targetIndex)
        {
            if (GameManager.Paused) return;

            var currentIndex = GameManager.Canvas.inventoryManager.GetIndexInventory();
            
            ItemDescription.Hide(); //esto modificarlo despues
            
            if (currentIndex == targetIndex || targetIndex == -1)
            {
                GameManager.Canvas.inventoryManager.InventorySwitcherUI.SwitchTo(0);
                GameManager.Canvas.inventoryManager.InventorySwitcherUI.gameObject.SetActive(false);
                GameManager.SetCursorVisibility(false);
                return;
            }
            
            GameManager.Canvas.inventoryManager.InventorySwitcherUI.SwitchTo(targetIndex);
            GameManager.Canvas.inventoryManager.InventorySwitcherUI.gameObject.SetActive(true);
            GameManager.SetCursorVisibility(true);
        }
    }
}