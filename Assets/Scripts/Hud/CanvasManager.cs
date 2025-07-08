using System.Collections;
using Effects;
using Inventory.Controller;
using Inventory.View;
using Items.Base;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Hud
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] public InventoryManager inventoryManager;
        [SerializeField] public TextMeshProUGUI LostUI;
        [SerializeField] public PausedMenu PauseMenu;
        [SerializeField] public RectTransform crosshairCrossbow;
        [SerializeField] public ToolbarUI toolbarUI;
        [SerializeField] private CanvasGroup continueCanvasGroup;
        
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
                GameManager.Canvas.inventoryManager.DeactiveBoltUI();
                return;
            }
            
            GameManager.Canvas.inventoryManager.InventorySwitcherUI.SwitchTo(targetIndex);
            
            GameManager.Canvas.inventoryManager.InventorySwitcherUI.gameObject.SetActive(true);
            GameManager.SetCursorVisibility(true);
            GameManager.Canvas.inventoryManager.ActiveBoltUI();
        }
        
        private IEnumerator Fade(CanvasGroup canvasGroup, float from, float to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
                yield return null;
            }
            canvasGroup.alpha = to;
        }

        public IEnumerator FinishGame()
        {
            StartCoroutine(Effect.Fade(continueCanvasGroup, 0, 1, 1));
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("Menu");
        }
    }
}