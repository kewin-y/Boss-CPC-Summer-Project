using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private LayerMask whatIsGround;
    private Vector2 motion;

    private Rigidbody2D rb2d;

    private bool targetDetected;

    public int damage;
    public float projectileMoveSpeed;

    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        Enemy.ProjectileMoveSpeed = projectileMoveSpeed;
    }
    //MAKE ABSTRACT CLASS FOR HEALTH BAR
    //IN DAMAGEABLE SCRIPT: EXTRACT TAKEDAMAGE() SO THAT IT CHANGES A UNIQUE HEALTH BAR FOR EACH POWER UP
    void OnCollisionEnter2D(Collision2D col) {

        Destroy(gameObject);

        GameObject target = col.gameObject;
        
        if (target.tag == "Player" || target.tag == "Shield") {
            Damageable targetScript = target.GetComponent<Damageable>();
            targetScript.TakeDamage(damage);
        }
    }
}