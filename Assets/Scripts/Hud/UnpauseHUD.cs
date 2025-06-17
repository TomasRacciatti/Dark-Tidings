using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class UnpauseHUD : MonoBehaviour
{
    public void Unpause()
    {
        GameManager.Pause(false);
    }
}
