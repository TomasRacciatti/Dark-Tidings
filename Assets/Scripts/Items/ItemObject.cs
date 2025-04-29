using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private bool equippable;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material[] materials;

        // Propiedades para acceder a los datos
        public string ItemName => itemName;
        public Sprite Image => image;
        public string Description => description;
        public int Stack => stack;
        public ItemType ItemType => type;
        public bool IsEquippable => equippable;
        public Mesh Mesh => mesh;
        public Material[] Materials => materials;
    }
}