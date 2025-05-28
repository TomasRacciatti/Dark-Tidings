using Managers;

namespace Characters.Player
{
    public class PlayerCharacter : Character
    {
        protected override void Awake()
        {
            base.Awake();
            _healthComponent.OnDeath += ShowGameOverScreen;
        }
        
        private void ShowGameOverScreen()
        {
            GameManager.Canvas.LostUI.gameObject.SetActive(true);
        }
    }
}