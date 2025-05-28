using System.Collections.Generic;
using Items.Base;

namespace Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, List<ItemAmount> modifiers = null);
        
        public void Heal(float healing, List<ItemAmount> modifiers = null);
    }
}