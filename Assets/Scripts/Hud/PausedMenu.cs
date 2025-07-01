using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public void Menu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
