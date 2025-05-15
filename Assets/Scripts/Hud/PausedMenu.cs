using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PausedMenu : MonoBehaviour
{
    private void Resume()
    {
        GameManager.Pause(false);
    }
    
    public void QuitGame()
    {
        GameManager.QuitGame();
    }
}
