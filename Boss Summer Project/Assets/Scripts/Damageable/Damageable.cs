using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An abstract class for anything that has a form of HP, and a way to get damaged
public abstract class Damageable : MonoBehaviour
{
    [Header("Max Health")]
    [SerializeField] protected float maxHealth;
    [SerializeField] private float maxAbsorptionHealth;
    public float MaxAbsorptionHealth { get => maxAbsorptionHealth; set => maxAbsorptionHealth = value; }

    #region Health
        protected float health;
        protected float absorptionHealth;
        public float AbsorptionHealth { get => absorptionHealth; set => absorptionHealth = value; }
    #endregion
    

    //Abstract method that removes a certain amount of the entity's health.
    //Must be implemented by the child class so the appropriate health bar can
    //be updated.
    public abstract void TakeDamage(float damage);

    //Abstract method that recovers a certain amount of the entity's health. 
    //Must be implemented by the child class so the appropriate health bar can
    //be updated.
    public abstract void Heal(int healAmount);

    public void ResetHealth() {
        health = maxHealth;
    }

    public abstract void Die();
}
