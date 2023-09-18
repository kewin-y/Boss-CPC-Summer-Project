using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private Transform player;

    private SwordItem swordItem;

    //When the sword is summoned, reset to max health and restore normal colour
    //Also, receive the sword script that is currently in effect so the sword health bar can be updated.
    
    public void Initialize(SwordItem swordItem) {
        //Set the new sword item and reset health
        this.swordItem = swordItem;
        //ResetHealth();
        VisualEffects.SetColor(gameObject, Color.white);
    }

    //For every frame that the player has the sword, follow the player and rotate towards the mouse
    void Update()
    {
        if (!PauseManager.IsPaused) {
            PointToMouse();
        }
        PointToMouse();
    }

    //Makes the sword follow the player
    //Rotates the sword to face towards the mouse position
    private void PointToMouse() {
        //Get the sword position, disregarding the -10 z-axis offset of the main camera
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        //Point the sword to the mouse
        transform.right = mousePosition - transform.position;
        
    }

    //Implement durability for the sword
    /*
    //Take damage from any type of hit while pulsing red
    public override void TakeDamage(float damage) {

        //Subtract damage from health and update the shield "health bar"
        health -= damage;
        shieldBoost.SetDurationLeft(health / maxHealth);

        VisualEffects.SetColor(gameObject, Color.red);
        if(isActiveAndEnabled) StartCoroutine(VisualEffects.FadeToColor(gameObject, 0.5f, Color.white));

        if (health <= 0)
            Die();
    }

    public override void Heal(int healAmount) {
        health += healAmount;
        
        //Health cannot overflow
        if (health > maxHealth)
            health = maxHealth;

        shieldBoost.SetDurationLeft(health / maxHealth);
    }

    //
    public override void Die() {
        shieldBoost.RemoveEffectFully();
        gameObject.SetActive(false);
        VisualEffects.SetColor(gameObject, Color.white);
    }
    */
}
