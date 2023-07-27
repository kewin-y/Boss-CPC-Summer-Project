using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Projectile;

    public Transform player;

    public float attackRadius;
    public float fireRate;

    [SerializeField] private bool canShoot;

    public static bool ProjectileTargeting;

    public bool canCollide;

    [SerializeField] private LayerMask whatIsPlayer;
    public GameObject deathEffect; 
    private PlayerController playerController;
    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        canShoot = true;
        ProjectileTargeting = false;
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

    // Update is called once per frame
    void Update()
    {
        bool collideWithPlayer = Physics2D.BoxCast(transform.position, new Vector2(0.45f, 0.45f), 0f, Vector2.down, 0f, whatIsPlayer);

        if(collideWithPlayer)
        {
            if(playerController.IsDashing)
            {
                die();
            }
        }
    }
    /*
    void FixedUpdate()
    {
        Vector2 dir = player.position - transform.position;

        if(dir.sqrMagnitude <= attackRadius && canShoot){
            StartCoroutine(shoot());
        }
    }
    */
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Kill")
        {
            Destroy(gameObject);
        }

    }

    void die()
    {
        GameObject deathParticles = Instantiate(deathEffect);
        deathParticles.transform.position = transform.position;

        StopAllCoroutines();
        gameObject.SetActive(false);

        ParticleSystem dpSystem = deathParticles.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule dpMain = dpSystem.main;

        dpMain.startColor = gameObject.GetComponent<SpriteRenderer>().color;
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