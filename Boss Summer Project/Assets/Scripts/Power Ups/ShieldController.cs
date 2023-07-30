using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : Damageable
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform player;
    [SerializeField] private Transform pivotPoint;

    void Start() {
        Die();
    }

    //
    void OnEnable()
    {
        ResetHealth();
    }

    // 
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

    public override void TakeDamage(int damage) {
        health -= damage;

        VisualEffects.SetColor(gameObject, Color.red);
        if(isActiveAndEnabled) StartCoroutine(VisualEffects.FadeToColor(gameObject, 0.5f, Color.white));

        if (health <= 0)
            Die();
    }

    public override void Die() {
        gameObject.SetActive(false);
    }
}
