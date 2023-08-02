using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject enemy;
    public GameObject projectile;

    public Transform player;

    public bool canCollide;
    public float moveSpeed;

    private Vector2 motion;

    private Rigidbody2D rb2d;
    private bool targetDetected;

    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    //MAKE ABSTRACT CLASS FOR HEALTH BAR
    //IN DAMAGEABLE SCRIPT: EXTRACT TAKEDAMAGE() SO THAT IT CHANGES A UNIQUE HEALTH BAR FOR EACH POWER UP
    void OnCollisionEnter2D(Collision2D col) {
        Destroy(gameObject);

        GameObject target = col.gameObject;
        
        if (target.tag == "Player" || target.tag == "Shield") {
            Damageable targetScript = target.GetComponent<Damageable>();
            targetScript.TakeDamage(5);
        }
    }
}