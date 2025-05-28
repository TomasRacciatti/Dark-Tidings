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
        [SerializeField] private LayerMask _layerMask;

        List<SO_Item> Modifiers => _modifiers;

        public void SetModifiers(List<SO_Item> modifiers)
        {
            _modifiers = modifiers;
        }

        public float force = 50;

        private Rigidbody rb;
        private bool _impacted = false;
        private Transform _parent;
        private CapsuleCollider _collider;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            _parent = transform.parent;
            _collider = GetComponent<CapsuleCollider>();
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
            if (_impacted) return;
            
            if (rb.velocity.sqrMagnitude > 0.01f)
            {
                transform.forward = rb.velocity.normalized;
            }

            var origin = transform.position - transform.forward * _collider.height / 2;
            var distance = rb.velocity.magnitude * Time.fixedDeltaTime;
            if (Physics.SphereCast(origin, _collider.radius, transform.forward, out RaycastHit hit, distance,
                    _layerMask)) Impact(hit);
        }

        private void Impact(RaycastHit hit)
        {
            if (_impacted) return;

            Debug.Log("Flecha impactó con: " + hit.collider.gameObject.name + "con Raycast");

            var health = hit.collider.GetComponent<IDamageable>();
            if (health != null)
            {
                health.TakeDamage(10, Modifiers);
            }

            rb.isKinematic = true;
            transform.position = hit.point - transform.forward * _collider.height / 3;
            transform.SetParent(hit.collider.transform);
            _impacted = true;
        }
    }
}