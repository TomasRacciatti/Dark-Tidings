using Managers;
using UnityEngine;

namespace Hud
{
    public class PausedMenu : MonoBehaviour
    {
        public void Resume()
        {
            GameManager.Pause(false);
        }
    
        public void QuitGame()
        {
            GameManager.QuitGame();
        }
    }
}
