using System;
using System.Collections.Generic;
using Interfaces;
using Items.Base;
using UnityEngine;

namespace Items
{
    public class Bolt : MonoBehaviour
    {
        [SerializeField] private List<ItemAmount> _modifiers = new();
        [SerializeField] private LayerMask _layerMask;

        List<ItemAmount> Modifiers => _modifiers;

        public void SetModifiers(List<ItemAmount> modifiers)
        {
            _modifiers = modifiers;
        }

        public float force = 50;

        private Rigidbody _rb;
        private bool _impacted;
        private Transform _parent;
        private CapsuleCollider _collider;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _parent = transform.parent;
            _collider = GetComponent<CapsuleCollider>();
        }

        private void OnEnable()
        {
            transform.parent = _parent;
            _impacted = false;
            _rb.isKinematic = false;
            _rb.velocity = transform.forward * force;
        }

        private void FixedUpdate()
        {
            if (_impacted) return;
            
            if (_rb.velocity.sqrMagnitude > 0.01f)
            {
                transform.forward = _rb.velocity.normalized;
            }

            var origin = transform.position - transform.forward * _collider.height / 3;
            var distance = _rb.velocity.magnitude * Time.fixedDeltaTime;
            if (Physics.SphereCast(origin, _collider.radius, transform.forward, out RaycastHit hit, distance,
                    _layerMask)) Impact(hit);
        }

        private void Impact(RaycastHit hit)
        {
            if (_impacted) return;

            Debug.Log("Flecha impactó con: " + hit.collider.gameObject.name + "con Raycast");

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            damageable?.TakeDamage(10, Modifiers);

            _rb.isKinematic = true;
            transform.position = hit.point - transform.forward * _collider.height / 3;
            transform.SetParent(hit.collider.transform);
            _impacted = true;
        }
    }
}