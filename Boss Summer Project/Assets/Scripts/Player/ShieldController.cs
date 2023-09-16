using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : Damageable
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform player;
    [SerializeField] private Transform pivotPoint;
    
    private ShieldBoost shieldBoost;

    //When the shield is summoned, reset to max health and restore normal colour
    //Also, receive the shield boost script that is currently in effect so the
    //shield health bar can be updated.
    public void Initialize(ShieldBoost shieldBoost) {
        //Before setting the new shield boost to the field, remove the current shield health bar from the UI
        if (this.shieldBoost != null)
            this.shieldBoost.RemoveFromUI();

        //Now, set the new shield boost and reset health
        this.shieldBoost = shieldBoost;
        ResetHealth();
        VisualEffects.SetColor(gameObject, Color.white);
    }

    //For every frame that the shield is alive, follow the player and rotate towards the mouse
    void Update()
    {
        if (!PauseManager.IsPaused) {
            FollowPlayer();
            PointToMouse();
        }
    }

    //Makes the shield follow the player
    private void FollowPlayer() {
        pivotPoint.position = player.position;
    }

    //Rotates the shield to face towards the mouse position
    private void PointToMouse() {
        //Get the mouse position, disregarding the -10 z-axis offset of the main camera
        Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        //Point the shield to the mouse
        pivotPoint.transform.right = mousePosition - pivotPoint.position;
    }

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
}
