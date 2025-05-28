using System.Collections.Generic;
using Items.Base;

namespace Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(int damage, LinkedList<ItemAmount> modifiers = null);
        
        public void Heal(int healing, LinkedList<ItemAmount> modifiers = null);
    }
}