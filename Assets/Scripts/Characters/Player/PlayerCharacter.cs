using Managers;

namespace Characters.Player
{
    public class PlayerCharacter : Character
    {
        protected override void Death()
        {
            GameManager.Canvas.LostUI.gameObject.SetActive(true);
        }
    }
}