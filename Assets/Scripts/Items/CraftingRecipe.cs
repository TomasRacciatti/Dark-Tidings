using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "Items/Crafting/Recipe")]
    public class CraftingRecipe : ScriptableObject
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