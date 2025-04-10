using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Custom/Item")]
    public class ItemObject : ScriptableObject
    {
        public Sprite image;
        public int stack = 10;
        public ItemType type;
    }

    public enum ItemType
    {
        Weapon,
        Tool,
        Armor,
        Material
    }
}