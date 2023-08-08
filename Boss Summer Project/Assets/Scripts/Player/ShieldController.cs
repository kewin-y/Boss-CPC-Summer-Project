using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : Damageable
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform player;
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private ShieldBoost shieldBoost;

    //When the shield is summoned, reset to max health and restore normal colour
    void OnEnable()
    {
        ResetHealth();
        VisualEffects.SetColor(gameObject, Color.white);
    }

    //For every frame that the shield is alive, follow the player and rotate towards the mouse
    private void Update()
    {
        FollowPlayer();
        PointToMouse();
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

    //
    public override void Die() {
        shieldBoost.RemoveEffectFully();
        gameObject.SetActive(false);
        VisualEffects.SetColor(gameObject, Color.white);
    }
}
