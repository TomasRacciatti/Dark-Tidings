using System.Collections.Generic;
using Items.Base;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(menuName = "ScriptableObject/Characters/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        [Header("Base Stats")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int maxStamina = 50;
        [SerializeField] private int attack = 10;
        [SerializeField] private float movementSpeed = 2.5f;
        [SerializeField] private float sprintMultiplier = 1.5f;
        [SerializeField] private float attackSpeed = 1f;
        [Header("Modifiers Stats")]
        [SerializeField] private SO_Item[] strengths;
        [SerializeField] private SO_Item[] weaknesses;
        
        //Propiedades
        public int MaxHealth => maxHealth;
        public int MaxStamina => maxStamina;
        public int Attack => attack;
        public float MovementSpeed => movementSpeed;
        public float SprintMultiplier => sprintMultiplier;
        public float AttackSpeed => attackSpeed;
        public IReadOnlyList<SO_Item> Strengths => strengths;
        public IReadOnlyList<SO_Item> Weaknesses => weaknesses;
    }
}