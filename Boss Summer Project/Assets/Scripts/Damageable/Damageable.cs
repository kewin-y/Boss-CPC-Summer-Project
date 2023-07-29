using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An abstract class for anything that has a form of HP, and a way to get damaged
public abstract class Damageable : MonoBehaviour
{
    [SerializeField] protected int maxHealth;

    protected int health;

    public abstract void TakeDamage(int damage);

    public void Heal(int healAmount) {
        health += healAmount;
    }

    public void ResetHealth() {
        health = maxHealth;
    }

    public abstract void Die();
}
