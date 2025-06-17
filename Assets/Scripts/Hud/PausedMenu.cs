using Managers;
using UnityEngine;

namespace Hud
{
    public class PausedMenu : MonoBehaviour
    {
        public void Resume()
        {
            GameManager.Pause(false);
            GameManager.Canvas.PauseMenu.gameObject.SetActive(false);
        }
        
        public void Restart()
        {
            GameManager.Restart();
        }
    
        public void QuitGame()
        {
            GameManager.QuitGame();
        }
    }
}
