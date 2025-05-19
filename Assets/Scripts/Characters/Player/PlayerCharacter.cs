using Managers;

namespace Characters.Player
{
    public class PlayerCharacter : Character
    {
        public static PlayerCharacter Instance;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        protected override void Death()
        {
            GameManager.Canvas.LostUI.gameObject.SetActive(true);
        }
    }
}