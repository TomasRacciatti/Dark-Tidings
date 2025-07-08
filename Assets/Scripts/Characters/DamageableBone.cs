using System;
using System.Collections;
using System.Collections.Generic;
using Features.Health;
using Interfaces;
using Items.Base;
using UnityEngine;

public class DamageableBone : MonoBehaviour, IDamageable
{
    [SerializeField] HealthComponent healthComponent;
    
    public void TakeDamage(float damage, List<ItemAmount> modifiers = null)
    {
        healthComponent.TakeDamage(damage, modifiers);
    }

    public void Heal(float healing, List<ItemAmount> modifiers = null)
    {
        healthComponent.Heal(healing, modifiers);
    }
}
