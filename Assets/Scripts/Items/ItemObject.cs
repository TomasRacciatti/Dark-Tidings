using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Default")]
    public class ItemObject : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite image;
        [SerializeField, TextArea] private string description = "ItemDescription";
        [SerializeField] private int stack = 10;
        [SerializeField] private ItemType type;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material[] materials;

        // Métodos virtuales para permitir override
        public virtual string GetItemName() => itemName;
        public virtual Sprite GetImage() => image;
        public virtual string GetDescription() => description;
        public virtual string GetFullDescription() => description;
        public virtual int GetStack() => stack;
        public virtual ItemType GetItemType() => type;
        public virtual Mesh GetMesh() => mesh;
        public virtual Material[] GetMaterials() => materials;
        public override string ToString() => GetItemName();
    }

    public enum ItemType
    {
        Weapon,
        Tool,
        Key,
        Armour,
        Projectile,
        MatMetal,
        MatResidue,
        MatInfusion,
        MatSpecial,
        Consumable,
        Throwable
    }
}