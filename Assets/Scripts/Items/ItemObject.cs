using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Custom/Item")]
    public class ItemObject : ScriptableObject
    {
        public string itemName;
        public Sprite image;
        public string description = "ItemDescription";
        public int stack = 10;
        public ItemType type;
        public Mesh mesh;
        public Material[] materials;
        
        public virtual float GetDamage()
        {
            return 0f;
        }
    }

    public enum ItemType
    {
        Weapon,
        Tool,
        Armour,
        Bullets,
        MatMetal,
        MatResidue,
        MatInfusion,
        MatSpecial,
        Consumable,
        Throwable
    }
}