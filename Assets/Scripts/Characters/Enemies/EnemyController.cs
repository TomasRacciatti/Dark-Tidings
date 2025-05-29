using System.Collections;
using Interfaces;
using Managers;
using Patterns;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float attackRange = 2f;
    
        Cooldown _cooldown = new Cooldown();
        NavMeshAgent _agent;
        Transform _target;
        Character _character;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _character = GetComponent<Character>();
            _agent.speed = _character.Stats.MovementSpeed;
        }

        private void Start()
        {
            StartCoroutine(ExecuteNextFrame());
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

        private void Update()
        {
            if (_target)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _target.position);

                if (distanceToTarget <= chaseRange)
                {
                    _agent.SetDestination(_target.position);
                }
                else
                {
                    _agent.ResetPath();
                }

                if (distanceToTarget <= attackRange && _cooldown.IsReady)
                {
                    GameManager.Player.GetComponent<IDamageable>().TakeDamage(25); //esta re mal esto
                    _cooldown.StartCooldown(1);
                }
            }
        }
    
    
    }
}
