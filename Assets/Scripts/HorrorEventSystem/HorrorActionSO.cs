using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HorrorActionSO  : ScriptableObject, IHorrorAction
{
    public abstract IEnumerator Execute();
}
