using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DamagedPlayer : MonoBehaviour
{
    public static DamagedPlayer instance;
    private CustomPassVolume customPassVolume;
    
    private void Awake()
    {
        instance = this;
        customPassVolume = GetComponent<CustomPassVolume>();
        Deactivate();
    }

    public static void Damaged()
    {
        instance.customPassVolume.customPasses[0].enabled = true;
        instance.CancelInvoke(nameof(Deactivate));
        instance.Invoke(nameof(Deactivate), 1);
    }

    private void Deactivate()
    {
        customPassVolume.customPasses[0].enabled = false;
    }
}
