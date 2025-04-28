using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/BoltObject")]
    public class BoltObject : ItemObject
    {
        [SerializeField] private List<ItemObject> crafted = new List<ItemObject>();
        
        public List<ItemObject> Crafted => crafted;
    }
}