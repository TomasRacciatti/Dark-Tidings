using System.Collections.Generic;
using Features.Health;
using Interfaces;
using Items.Base;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(HealthComponent))]
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private float speed = 3;

        [SerializeField] private SO_Item[] strengths;
        [SerializeField] private SO_Item[] weaknesses;

        protected HealthComponent _healthComponent;
        
        public float Speed => speed;
        private IReadOnlyList<SO_Item> Strengths => strengths;
        private IReadOnlyList<SO_Item> Weaknesses => weaknesses;
        public HealthComponent HealthComponent => _healthComponent;
        
        protected virtual void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.Initialize(Strengths, Weaknesses);
        }
    }
}