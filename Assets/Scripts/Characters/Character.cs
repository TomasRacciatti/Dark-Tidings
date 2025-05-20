using System.Collections.Generic;
using Interfaces;
using Items.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
        public int health = 100;
        public int maxHealth = 100;
        public float speed = 3;

        public List<SO_Item> strengths;
        public List<SO_Item> weaknesses;

        private void Awake()
        {
            health = maxHealth;
        }
        
        public virtual void TakeDamage(int damage, List<SO_Item> modifiers = null)
        {
            float finalDamage = damage;

            if (modifiers != null && modifiers.Count > 0)
            {
                foreach (var modifier in modifiers)
                {
                    if (weaknesses.Contains(modifier))
                    {
                        finalDamage *= 2.5f;
                    }
                    else if (strengths.Contains(modifier))
                    {
                        finalDamage *= 0.2f;
                    }
                }
            }

            health -= Mathf.RoundToInt(finalDamage);

            Debug.Log($"{gameObject.name} recibió {finalDamage} de daño");

            if (health <= 0)
            {
                Death();
            }
        }

        public virtual void Heal(int healing, List<SO_Item> modifiers = null)
        {
            float finalHealing = healing;

            if (modifiers != null && modifiers.Count > 0)
            {
                foreach (var modifier in modifiers)
                {
                    if (strengths.Contains(modifier))
                    {
                        finalHealing *= 1.5f;
                    }
                    else if (weaknesses.Contains(modifier))
                    {
                        finalHealing *= 0.5f;
                    }
                }
            }

            health += Mathf.RoundToInt(finalHealing);

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            Debug.Log($"{gameObject.name} se curó {finalHealing} puntos");
        }
        
        protected abstract void Death();
    }
}