using UnityEngine;
using Inventory;
using Items;

public abstract class Item : MonoBehaviour
{
    [SerializeField] public ItemObject itemObject;
    
    public abstract void StartUsing();
    public abstract void StopUsing();
    public abstract void AlternativeUse();
}
