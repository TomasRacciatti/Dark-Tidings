using System;
using Characters.Player;
using Hud;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject eventSystemPrefab;
        
        public static PlayerCharacter Player { get; private set; }
        public static CanvasManager Canvas { get; private set; }
        public static GameManager Instance { get; private set; }
        
        public static bool Paused { get; private set; }

        private void Awake()
        {
            InstantiateInstance();
        }

        private void Start()
        {
            Instantiate(eventSystemPrefab);
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                SpawnPlayer();
                SetCursorVisibility(false);
            }
        }

        private void InstantiateInstance()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        
        private void SpawnPlayer()
        {
            Transform spawnPoint = SpawnPointManager.Instance.GetDefault;
            
            Player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation)
                .GetComponentInChildren<PlayerCharacter>();
            Canvas = Instantiate(hudPrefab).GetComponent<CanvasManager>();
        }
        
        public static void Pause(bool paused)
        {
            if (paused)
            {
                Paused = true;
                Time.timeScale = 0;
                Canvas.PauseMenu.gameObject.SetActive(true);
                SetCursorVisibility(true);
            }
            else
            {
                Paused = false;
                Time.timeScale = 1;
                Canvas.PauseMenu.gameObject.SetActive(false);
                SetCursorVisibility(false);
            }
        }

        public static void Restart()
        {
            Pause(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void TogglePause()
        {
            Paused = !Paused;
            Pause(Paused);
        }
        
        public static void SetCursorVisibility(bool visible = false)
        {
            if (visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
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
