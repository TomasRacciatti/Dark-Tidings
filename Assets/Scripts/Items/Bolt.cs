using System;
using System.Collections.Generic;
using Interfaces;
using Items.Base;
using Patterns.ObjectPool;
using UnityEngine;

namespace Items
{
    public class Bolt : MonoBehaviour
    {
        [SerializeField] private List<SO_Item> _modifiers = new();
    
        List<SO_Item> Modifiers => _modifiers;

        public void SetModifiers(List<SO_Item> modifiers)
        {
            _modifiers = modifiers;
        }

        public float force = 50;
        
        private Rigidbody rb;
        private bool _impacted = false;
        private Transform _parent;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            _parent = transform.parent;
        }

        private void OnEnable()
        {
            transform.parent = _parent;
            _impacted = false;
            rb.isKinematic = false;
            rb.velocity = transform.forward * force;
        }
        
        private void FixedUpdate()
        {
            if (!_impacted && rb.velocity.sqrMagnitude > 0.01f)
            {
                transform.forward = rb.velocity.normalized;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_impacted)
            {
                Debug.Log("Flecha impactó con: " + other.gameObject.name);
                
                var health = other.gameObject.GetComponent<IDamageable>();
                if (health != null)
                {
                    health.TakeDamage(10, Modifiers);
                }
                
                rb.isKinematic = true;
                transform.SetParent(other.transform);
                //Destroy(gameObject, 10f);
                _impacted = true;
            }
        }
    }
}
