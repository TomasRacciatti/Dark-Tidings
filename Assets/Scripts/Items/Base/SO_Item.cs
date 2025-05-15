using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    [CreateAssetMenu(menuName = "ScriptableObject/Items/Item")]
    public class SO_Item : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite image;
        [SerializeField, TextArea] private string description = "ItemDescription";
        [SerializeField] private string modifierName;
        [SerializeField] private int stack = 10;
        [SerializeField] private ItemType type;
        [SerializeField] private bool equippable;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material[] materials;

        // Propiedades para acceder a los datos
        public string ItemName => itemName;
        public Sprite Image => image;
        public string Description => description;
        public string ModifierName => modifierName;
        public int Stack => stack;
        public ItemType ItemType => type;
        public bool IsEquippable => equippable;
        public Mesh Mesh => mesh;
        public Material[] Materials => materials;
    }
}