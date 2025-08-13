using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LightToggleAction", menuName = "ScriptableObject/HorrorEvents/Actions/Light Toggle")]
public class LightToggleAction : HorrorActionSO
{
    public LightController.Mode modeToSet = LightController.Mode.Off;
    
    public IEnumerator ExecuteOn(LightController controller)
    {
        if (controller == null)
            yield break;

        controller.SwitchMode(modeToSet);
        yield break;
    }

    public override IEnumerator Execute()
    {
        throw new System.NotSupportedException(
            $"{nameof(LightToggleAction)} must be run via ExecuteOn(controller)");
    }
}
