using System.Collections.Generic;
using Inventory;
using Items.Base;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObject/Items/Crafting/Recipe")]
    public class SO_Recipe : ScriptableObject
    {
        public List<ItemAmount> ingredients;
        public ItemAmount resultItem;
    }
}