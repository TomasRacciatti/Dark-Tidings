using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class GoldBullet : ItemDecorator
    {
        private float bonusDamage = 5f;

        public GoldBullet(ItemObject item) : base(item) { }

        public override float GetDamage()
        {
            return base.GetDamage() + bonusDamage;
        }
    }
}
