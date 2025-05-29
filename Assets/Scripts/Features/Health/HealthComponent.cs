using System;
using System.Collections.Generic;
using Interfaces;
using Items.Base;
using UnityEngine;

namespace Features.Health
{
    [DisallowMultipleComponent]
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        private float _maxHealth = 100f;
        private float _currentHealth;
        
        private HashSet<SO_Item> _strengths = new();
        private HashSet<SO_Item> _weaknesses = new();

        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public bool IsDead => _currentHealth <= 0;
        
        public event Action<float, List<ItemAmount>> OnDamaged;
        public event Action<float, List<ItemAmount>> OnHealed;
        public event Action<float, float> OnHealthChanged; // current, max
        public event Action OnDeath;
        public event Action OnRevived;
        
        private void Start()
        {
            _currentHealth = _maxHealth;
            NotifyHealthChanged();
        }
        
        public void Initialize(float maxHealth ,IEnumerable<SO_Item> strengths, IEnumerable<SO_Item> weaknesses)
        {
            _strengths = new HashSet<SO_Item>(strengths);
            _weaknesses = new HashSet<SO_Item>(weaknesses);
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
            NotifyHealthChanged();
        }
        
        public void TakeDamage(float damage, List<ItemAmount> modifiers = null)
        {
            if (IsDead) return;
            
            ApplyModifiers(ref damage, modifiers, false);

            _currentHealth = Mathf.Max(_currentHealth - damage, 0f);
            OnDamaged?.Invoke(damage, modifiers);
            NotifyHealthChanged();

            if (IsDead)
            {
                HandleDeath();
            }
        }

        public void Heal(float healing, List<ItemAmount> modifiers = null)
        {
            if (IsDead) return;
            
            ApplyModifiers(ref healing, modifiers, true);

            _currentHealth = Mathf.Min(_currentHealth + healing, _maxHealth);
            OnHealed?.Invoke(healing, modifiers);
            NotifyHealthChanged();
        }

        public void Revive()
        {
            if (!IsDead) return;

            OnRevived?.Invoke();
            _currentHealth = _maxHealth;
            NotifyHealthChanged();
        }

        private void NotifyHealthChanged()
        {
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private void HandleDeath()
        {
            OnDeath?.Invoke();
        }
        
        private void ApplyModifiers(ref float value, List<ItemAmount> modifiers, bool isPositive)
        {
            if (modifiers == null || modifiers.Count == 0) return;

            foreach (var modifier in modifiers)
            {
                if (_weaknesses.Contains(modifier.SoItem))
                {
                    value *= isPositive ? 0.75f : 2.5f;
                }
                else if (_strengths.Contains(modifier.SoItem))
                {
                    value *= isPositive ? 1.25f : 0.2f;
                }
            }
        }
    }
}