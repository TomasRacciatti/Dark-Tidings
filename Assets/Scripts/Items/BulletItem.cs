using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Custom/Items/Bullet")]
    public class BulletItem : ItemObject
    {
        public float baseDamage = 5f;

        public override float GetDamage()
        {
            return baseDamage;
        }
    }
}
