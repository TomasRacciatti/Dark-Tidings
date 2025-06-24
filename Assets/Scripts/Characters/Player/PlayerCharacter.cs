using Inventory.Model;
using Items.Base;
using Managers;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerCharacter : Character
    {
        public InventorySystem inventory;
        private PlayerView _playerView;
        public PlayerController playerController;
        
        protected override void Awake()
        {
            base.Awake();
            playerController = GetComponent<PlayerController>();
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

        public void AddItem(ref ItemAmount itemAmount)
        {
            inventory.AddItem(ref itemAmount);
            if (itemAmount.IsEmpty) return;
            ItemDropper.Drop(itemAmount);
        }
    }
}