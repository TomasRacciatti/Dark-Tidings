using Characters.Player;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public static bool Paused { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        
        public static void Pause(bool paused)
        {
            if (paused)
            {
                Paused = true;
                Time.timeScale = 0;
                CanvasManager.Instance.PauseMenu.gameObject.SetActive(true);
            }
            else
            {
                Paused = false;
                Time.timeScale = 1;
                CanvasManager.Instance.PauseMenu.gameObject.SetActive(false);
            }
        }

        public static void TogglePause()
        {
            Paused = !Paused;
            Pause(Paused);
        }
    
        public static void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}
