namespace Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(int damage);
        
        public void Heal(int healing);
    }
}