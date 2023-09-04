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
    protected virtual void OnCollisionEnter2D(Collision2D col) {
        target = col.gameObject;

 
        Destroy(gameObject);


        if (target.tag == "Player" || target.tag == "Shield") {
            Damageable targetScript = target.GetComponent<Damageable>();
            targetScript.TakeDamage(damage);
        }
    }
}