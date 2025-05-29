using Inventory.Model;
using Managers;

namespace Characters.Player
{
    public class PlayerCharacter : Character
    {
        public InventorySystem inventory;
        
        protected override void Awake()
        {
            base.Awake();
            inventory = GetComponent<InventorySystem>();
            _healthComponent.OnDeath += ShowGameOverScreen;
        }
        
        private void ShowGameOverScreen()
        {
            GameManager.Canvas.LostUI.gameObject.SetActive(true);
        }
    }
}