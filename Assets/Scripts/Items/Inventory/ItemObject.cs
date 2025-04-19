using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Custom/Item")]
    public class ItemObject : ScriptableObject
    {
        public Sprite image;
        public string description = "ItemDescription";
        public int stack = 10;
        public ItemType type;
        public Mesh mesh;
        public Material[] materials;
    }

    public enum ItemType
    {
        Weapon,
        Tool,
        Armour,
        MatMetal,
        MatResidue,
        MatInfusion,
        MatSpecial,
        Consumable,
        Throwable
    }
}