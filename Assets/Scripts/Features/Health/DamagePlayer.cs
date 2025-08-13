using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.HealthComponent.TakeDamage(20);
        }
    }
}
