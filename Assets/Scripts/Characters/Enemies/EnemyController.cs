using System.Collections;
using System.Collections.Generic;
using Features.Health;
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

        [SerializeField] private AudioClip zombieClip;
        [SerializeField] private AudioClip hitClip;
        
        bool isAttacking = false;
        bool isHitting = false;
        
        private float extraSpeed = 0;
        
        NavMeshAgent _agent;
        Transform _target;
        Character _character;
        Animator _animator;
        CapsuleCollider _collider;
        AudioSource _audioSource;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _character = GetComponent<Character>();
            _collider = GetComponentInChildren<CapsuleCollider>();
            _audioSource = GetComponent<AudioSource>();
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

        public void ActivateDamage()
        {
            handCollider.gameObject.SetActive(true);
            Rotate();
            transform.position += transform.forward * (1 + extraSpeed) * 0.15f;
        }
        
        public void DeactivateDamage()
        {
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
            if (active)
            {
                _animator.SetTrigger("Activate");
                _audioSource.clip = zombieClip;
                _audioSource.Play();
                GetComponent<HealthComponent>().inmune = false;
                Invoke(nameof(Activate), 2f);
            }
            else
            {
                isActive = false;
            }
            _collider.enabled = active;
        }

        private void Activate()
        {
            isActive = true;
        }
        
        private void Attack()
        {
            if (isAttacking) return;
            isAttacking = true;
            _animator.SetTrigger("Attack");
            Invoke(nameof(StopAttacking), 0.9f);
        }

        private void StopAttacking()
        {
            isAttacking = false;
        }

        private void Hit(float damage, List<ItemAmount> modifiers)
        {
            foreach (ItemAmount modifier in modifiers)
            {
                foreach (var i in _character.Stats.Strengths)
                {
                    if (i == modifier.SoItem)
                    {
                        AddSpeed();
                    }
                }
            }
            _audioSource.PlayOneShot(hitClip);
            isHitting = true;
            _animator.SetTrigger("Hit");
            Invoke(nameof(StopHitting), 1.3f);
        }

        private void AddSpeed()
        {
            extraSpeed += 0.2f;
            _agent.speed = _character.Stats.MovementSpeed + extraSpeed;
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
            StartCoroutine(FadeOutAudio(1f));
            
            Invoke(nameof(Deactivate), 5);
        }
        
        private IEnumerator FadeOutAudio(float duration)
        {
            float startVol = _audioSource.volume;
            float elapsed  = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(startVol, 0f, elapsed / duration);
                yield return null;
            }

            _audioSource.volume = 0f;
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
                Rotate();
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

        private void Rotate()
        {
            Vector3 direction = _target.position - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
            {
                Vector3 euler = Quaternion.LookRotation(direction.normalized, Vector3.up).eulerAngles;
                transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
            }
        }
    }
}
