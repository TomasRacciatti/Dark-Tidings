using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource _audioSource;
    
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.Play();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void Play()
    {
        SceneManager.LoadScene("DT_Farmhouse");
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
