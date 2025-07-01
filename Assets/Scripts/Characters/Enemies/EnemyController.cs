using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Items.Base;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float attackRange = 1f;

        [SerializeField] private bool isActive = true;
        [SerializeField] private GameObject handCollider;
        
        bool isAttacking = false;
        bool isHitting = false;
        
        NavMeshAgent _agent;
        Transform _target;
        Character _character;
        Animator _animator;
        CapsuleCollider _collider;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _character = GetComponent<Character>();
            _collider = GetComponent<CapsuleCollider>();
            _agent.speed = _character.Stats.MovementSpeed;
        }

        private void Start()
        {
            StartCoroutine(ExecuteNextFrame());
            _character.HealthComponent.OnDeath += Die;
            _character.HealthComponent.OnDamaged += Hit;
            _collider.enabled = isActive;
            handCollider.gameObject.SetActive(false);
        }
    
        private IEnumerator ExecuteNextFrame()
        {
            yield return null;
            SetTarget();
        }

        private void SetTarget()
        {
            _target = GameManager.Player.transform;
        }
        
        public void SetActive(bool active)
        {
            isActive = active;
            if (active)
            {
                _animator.SetTrigger("Activate");
            }
            _collider.enabled = active;
        }
        
        private void Attack()
        {
            if (isAttacking) return;
            isAttacking = true;
            _animator.SetTrigger("Attack");
            handCollider.gameObject.SetActive(true);
            Invoke(nameof(StopAttacking), 1.7f);
        }

        private void StopAttacking()
        {
            isAttacking = false;
            handCollider.gameObject.SetActive(false);
        }

        private void Hit(float damage, List<ItemAmount> modifiers)
        {
            isHitting = true;
            _animator.SetTrigger("Hit");
            Invoke(nameof(StopHitting), 1.2f);
        }

        private void StopHitting()
        {
            isHitting = false;
        }

        private void Die()
        {
            SetActive(false);
            handCollider.gameObject.SetActive(false);
            _agent.ResetPath();
            _animator.SetBool("Walking", false);
            _animator.ResetTrigger("Hit");
            _animator.SetTrigger("Die");
            Invoke(nameof(Deactivate), 5);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!isActive) return;
            if (isAttacking || isHitting)
            {
                _agent.ResetPath();
                _animator.SetBool("Walking", false);
                return;
            }
            
            if (_target)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _target.position);

                if (distanceToTarget <= chaseRange)
                {
                    _agent.SetDestination(_target.position);
                    _animator.SetBool("Walking", true);
                }
                else
                {
                    _agent.ResetPath();
                    _animator.SetBool("Walking", false);
                }

                if (distanceToTarget <= attackRange)
                {
                    Attack();
                }
            }
        }
    }
}
