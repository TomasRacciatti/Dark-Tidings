using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Player;
using Managers;
using Patterns;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange = 2f;
    
    Cooldown _cooldown = new Cooldown();
    NavMeshAgent agent;
    Transform target;
    Character character;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<Character>();
        agent.speed = character.speed;
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
        target = GameManager.Player.transform;
    }

    private void Update()
    {
        if (target)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= chaseRange)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                agent.ResetPath();
            }

            if (distanceToTarget <= attackRange && _cooldown.IsReady)
            {
                GameManager.Player.GetComponent<Character>().TakeDamage(25); //esta re mal esto
                _cooldown.StartCooldown(1);
            }
        }
    }
    
    
}
