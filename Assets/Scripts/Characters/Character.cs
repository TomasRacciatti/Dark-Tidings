using Interfaces;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    public int health = 100;
    public int maxHealth = 100;
    public float speed = 3;

    private PlayerBackpackHandler _backpackHandler;

    private void Awake()
    {
        health = maxHealth;
        _backpackHandler = GetComponent<PlayerBackpackHandler>();
    }

    public virtual void TakeDamage(int damage)
    {
        // If this character has a backpack handler (is a player with a backpack), 
        // let it modify the damage amount
        if (_backpackHandler != null)
        {
            //_backpackHandler.ModifyIncomingDamage(ref damage);
        }

        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    public void Heal(int healing)
    {
        health += healing;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    protected abstract void Death();
}