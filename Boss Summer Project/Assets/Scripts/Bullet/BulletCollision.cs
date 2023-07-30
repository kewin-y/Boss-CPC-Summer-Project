using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {

        Damageable damageableScript = col.gameObject.GetComponent<Damageable>();

        if(damageableScript)
        {
            damageableScript.TakeDamage(15); 
        }

        gameObject.SetActive(false);

    }
}