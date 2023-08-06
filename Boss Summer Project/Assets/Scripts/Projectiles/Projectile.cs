using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    [SerializeField] protected float damage;
    protected GameObject target;

    void Start()
    {
    
    }
    //MAKE ABSTRACT CLASS FOR HEALTH BAR
    //IN DAMAGEABLE SCRIPT: EXTRACT TAKEDAMAGE() SO THAT IT CHANGES A UNIQUE HEALTH BAR FOR EACH POWER UP
    void OnCollisionEnter2D(Collision2D col) {
        target = col.gameObject;

        if(!(target.tag == "Enemy")) {
            Destroy(gameObject);
        } 

        if (target.tag == "Player" || target.tag == "Shield") {
            Damageable targetScript = target.GetComponent<Damageable>();
            targetScript.TakeDamage(damage);
            print(damage);
        }
    }
}