using Inventory.Model;
using Managers;

namespace Characters.Player
{
    public class PlayerCharacter : Character
    {
        public InventorySystem inventory;
        private PlayerView _playerView;
        
        protected override void Awake()
        {
            base.Awake();
            inventory = GetComponent<InventorySystem>();
            _healthComponent.OnDeath += ShowGameOverScreen;
            _playerView  = GetComponent<PlayerView>();

            _healthComponent.OnDamaged += (damage, modifiers) =>
                _playerView.Damaged();
        }

        
        private void ShowGameOverScreen()
        {
            GameManager.Canvas.LostUI.gameObject.SetActive(true);
        }
    }
}