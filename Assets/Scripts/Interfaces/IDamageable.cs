namespace Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(int damage);
        
        void Heal(int healing);
    }
}