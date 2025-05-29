using System.Collections.Generic;
using Features.Health;
using Interfaces;
using Items.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
    [RequireComponent(typeof(HealthComponent))]
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private CharacterStats stats;
        protected HealthComponent _healthComponent;
        
        public CharacterStats Stats => stats;
        public HealthComponent HealthComponent => _healthComponent;
        
        protected virtual void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.Initialize(Stats.MaxHealth, Stats.Strengths, Stats.Weaknesses);
        }
    }
}