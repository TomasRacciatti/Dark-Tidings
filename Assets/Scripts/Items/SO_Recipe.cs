using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObject/Items/Crafting/Recipe")]
    public class SO_Recipe : ScriptableObject
    {
        [System.Serializable]
        public class Ingredient
        {
            public ItemAmount item;
        }

        public List<ItemAmount> ingredients;
        public ItemAmount resultItem;
    }
}