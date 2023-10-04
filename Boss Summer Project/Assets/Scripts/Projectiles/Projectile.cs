using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected Vector2 projectileSize;
    // Ok so. We can get projectile size by getting reference to boxcollider but that is inefficient so lets just hard-code it 
    [SerializeField] protected LayerMask playerLayers;

    protected bool hasCollided;

    protected virtual void OnEnable()
    {
        hasCollided = false;
    }

    protected virtual void Update()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, projectileSize, transform.rotation.eulerAngles.z, playerLayers);
        if (collider && !hasCollided)
        {
            hasCollided = true;
            DoCollision(collider);
            Destroy(gameObject);
        }
    }

    protected virtual void DoCollision(Collider2D collider)
    {
        Damageable targetScript = collider.GetComponent<Damageable>();
        targetScript.TakeDamage(damage);
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }

}