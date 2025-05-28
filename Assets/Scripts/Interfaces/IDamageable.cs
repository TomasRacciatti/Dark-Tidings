using System.Collections.Generic;
using Items.Base;

namespace Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(int damage, List<SO_Item> modifiers = null);
        
        public void Heal(int healing, List<SO_Item> modifiers = null);
    }
}