using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTJ : MonoBehaviour
{
    public GameObject Projectile;

    public Transform player;

    public float attackRadius;
    public float fireRate;

    [SerializeField] private bool canShoot;

    public static bool ProjectileTargeting;

    public bool canCollide;

    private Rigidbody2D rb2d;   
    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        ProjectileTargeting = false;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called every fixed timestep
    // Used for physics
    void FixedUpdate()
    {
        Vector2 dir = player.position - transform.position;

        if(dir.sqrMagnitude <= attackRadius && canShoot){
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        Vector3 left = new Vector3(-1,0,0);
        canShoot = false;
        GameObject bullet = Instantiate(Projectile) as GameObject;
        bullet.transform.position = transform.position+left;
        bullet.SetActive(true);
        yield return new WaitForSeconds(1/fireRate);
        canShoot = true;
        yield return new WaitForSeconds(10 - 1/fireRate);
        bullet.SetActive(false);
    }
}