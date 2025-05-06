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
            CanvasGameManager.Instance.LostUI.gameObject.SetActive(true);
        }
    }
}