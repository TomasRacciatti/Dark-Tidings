using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LightToggleAction", menuName = "ScriptableObject/HorrorEvents/Actions/Light Toggle")]
public class LightToggleAction : HorrorActionSO
{
    [Tooltip("What mode to switch the light into")]
    public Mode lightMode = Mode.On;

    [Tooltip("If Flicker, how fast (cycles per second)")]
    public float flickerRate = 5f; // Modificar

    
    public IEnumerator ExecuteOn(Light target)
    {
        if (target == null) yield break;

        switch(lightMode)
        {
            case Mode.On:
                target.enabled = true;
                break;
            case Mode.Off:
                target.enabled = false;
                break;
            case Mode.Flicker:
                // simple example: flip on/off at flickerRate
                float interval = 1f / flickerRate;
                // flicker for one second, then stop
                float end = Time.time + 1f;
                while (Time.time < end)
                {
                    target.enabled = !target.enabled;
                    yield return new WaitForSeconds(interval);
                }
                break;
        }
    }

    public override IEnumerator Execute()
    {
        throw new System.NotImplementedException();
    }
    
    public enum Mode
    {
        On,
        Off,
        Flicker
    }
}
