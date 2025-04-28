using Inventory.Model;
using Inventory.View;
using UnityEngine;

namespace Inventory.Controller
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] public GameObject itemPrefab;
        
        //Slots
        [SerializeField] public Toolbar toolbar;
        [SerializeField] public InventoryView inventoryView;
        
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject backpackUI;

        public void ToggleInventory(bool active)
        {
            inventoryUI.gameObject.SetActive(active);
            if (active)
            {
                // Mostrar y liberar el cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Ocultar y bloquear el cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        
        public void ToggleBackpack(bool active)
        {
            backpackUI.gameObject.SetActive(active);
        }
    }
}