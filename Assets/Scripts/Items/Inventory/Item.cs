using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public abstract void StartUsing();
    public abstract void StopUsing();
    public abstract void AlternativeUse();
}
