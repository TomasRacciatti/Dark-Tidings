using Interfaces;
using UnityEngine;

namespace Characters
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
        public int health = 100;
        public int maxHealth = 100;
        public float speed = 3;

        private void Awake()
        {
            health = maxHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Death();
            }
        }

        public void Heal(int healing)
        {
            health += healing;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        protected abstract void Death();
    }
}