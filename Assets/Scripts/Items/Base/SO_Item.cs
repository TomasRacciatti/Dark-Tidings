using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Base
{
    [CreateAssetMenu(menuName = "ScriptableObject/Items/Item")]
    public class SO_Item : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite image;
        [SerializeField, TextArea] private string description = "ItemDescription";
        [SerializeField] private string modifierName;
        [SerializeField] private int modifierPriority = 0;
        [SerializeField, Min(1)] private int stack = 10;
        [SerializeField] private ItemType type;
        [SerializeField] private bool equippable;
        [SerializeField] private SO_Item ammoType;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material[] materials;
        [SerializeField] private AudioClip grabAudioClip;

        [SerializeField] private itemImage[] imageDictionary = Array.Empty<itemImage>();

        // Propiedades para acceder a los datos
        public string ItemName => itemName;
        //public Sprite Image => image;
        public string Description => description;
        public string ModifierName => modifierName;
        public int ModifierPriority => modifierPriority;
        public int Stack => stack;
        public ItemType ItemType => type;
        public bool IsEquippable => equippable;
        public SO_Item AmmoType => ammoType;
        public bool HasAmmo => ammoType != null;
        public Mesh Mesh => mesh;
        public Material[] Materials => materials;
        public AudioClip AudioClip => grabAudioClip;

        public Sprite Image2(List<ItemAmount> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                foreach (var image in imageDictionary)
                {
                    if (modifier.SoItem == image.item)
                    {
                        return image.image;
                    }
                }
            }
            
            return image;
        }
    }

    [Serializable]
    public struct itemImage
    {
        [SerializeField] public SO_Item item;
        [SerializeField] public Sprite image;
    }
}