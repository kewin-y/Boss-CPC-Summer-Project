using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private LayerMask whatIsGround;
    private Rigidbody2D rb2d;

    public float ProjectileMoveSpeed;
    
    public int damage;

    public Enemy enemyScript;

    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        enemyScript = enemy.GetComponent<Enemy>();
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
    public void shoot(){
        enemyScript.distanceFromPlayer = (enemyScript.transform.position - enemyScript.player.transform.position).magnitude;
        enemyScript.timeFromPlayer = enemyScript.distanceFromPlayer/ProjectileMoveSpeed;
        enemyScript.futurePlayerPosition = (Vector2)enemyScript.player.transform.position + enemyScript.timeFromPlayer * enemyScript.player_rb2d.velocity;
        enemyScript.projectileDirection = (enemyScript.futurePlayerPosition - (Vector2)enemy.transform.position).normalized; // That's clever -kevin
        
        transform.position = (Vector2)transform.position + enemyScript.projectileDirection;
        rb2d.velocity = enemyScript.projectileDirection * ProjectileMoveSpeed * Time.deltaTime;
    }
}