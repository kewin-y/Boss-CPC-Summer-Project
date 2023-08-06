using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    [SerializeField] private float damage;
    [SerializeField] protected bool isBomb;
    [SerializeField] private float bombRange;
    [SerializeField] private GameObject explosion;

    void Start()
    {
    
    }
    //MAKE ABSTRACT CLASS FOR HEALTH BAR
    //IN DAMAGEABLE SCRIPT: EXTRACT TAKEDAMAGE() SO THAT IT CHANGES A UNIQUE HEALTH BAR FOR EACH POWER UP
    void OnCollisionEnter2D(Collision2D col) {

        if(!(col.gameObject.tag == "Enemy")) {
            Destroy(gameObject);
        }

        GameObject target = col.gameObject;

        if (isBomb) {
            //If the bomb directly hits a damageable entity, it applies the full 100 damage
            Damageable directHitScript = target.GetComponent<Damageable>();
            if (directHitScript != null) {
                directHitScript.TakeDamage(damage);
            }

            //The bomb also applies damage to all entities within its 2 unit range depending on the distance
            RaycastHit2D[] allEntitiesInRange = Physics2D.CircleCastAll(transform.position, bombRange, new Vector2(0,0), 0f);

            foreach (RaycastHit2D entity in allEntitiesInRange) {

                Damageable targetScript = entity.collider.gameObject.GetComponent<Damageable>();
                if(targetScript != null && targetScript != directHitScript) {

                    float distanceBetweenBombAndTarget = (entity.transform.position - transform.position).magnitude;
                    float damagePercentage = (bombRange-distanceBetweenBombAndTarget)/bombRange;

                    if (damagePercentage < 0)
                        damagePercentage = 0;
                    
                    targetScript.TakeDamage(damagePercentage * damage);
                }
            }
            GameObject explosionParticles = Instantiate(explosion) as GameObject;
            explosionParticles.transform.position = transform.position;
            Destroy(explosionParticles, 2f);
        } else if (target.tag == "Player" || target.tag == "Shield") {
            Damageable targetScript = target.GetComponent<Damageable>();
            targetScript.TakeDamage(damage);
        }
    }
}