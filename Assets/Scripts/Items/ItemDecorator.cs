using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public abstract class ItemDecorator : ItemObject
    {
        protected ItemObject item;
        
        public ItemDecorator(ItemObject item)
        {
            this.item = item;
            this.itemName = item.itemName;
            this.image = item.image;
            this.description = item.description;
            this.stack = item.stack;
            this.type = item.type;
            this.mesh = item.mesh;
            this.materials = item.materials;
        }

        public override float GetDamage()
        {
            return item.GetDamage();
        }
    }
}