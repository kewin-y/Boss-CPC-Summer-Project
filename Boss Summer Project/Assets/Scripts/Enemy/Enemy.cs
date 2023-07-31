using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float attackRadius;
    public float fireRate;

    public static bool ProjectileTargeting;
    
    public GameObject particles;

    [SerializeField] private Vector3 spawnPoint;

    private bool canCollide = true;
    public bool CanCollide {
        get{return canCollide;}
        set{canCollide = value;}
    }
    public GameObject Projectile;

    public Transform player;

    [SerializeField] private bool canShoot;

    [SerializeField] private LayerMask whatIsPlayer;
    public GameObject deathEffect; 
    private PlayerController playerController;
    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;

    // Start is called before the first frame update
    void Start()
    {
        canCollide = true;
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
    //This method will run when the player collides with something
    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Kill")
        {
            die();
        }

    }
    void respawn(){
        gameObject.SetActive(true);
        transform.position = spawnPoint;
    }
    IEnumerator shoot()
    {
        canShoot = false;
        GameObject bullet = Instantiate(Projectile) as GameObject;
        bullet.transform.position = transform.position + Vector3.left;
        bullet.SetActive(true);
        yield return new WaitForSeconds(1/fireRate);
        canShoot = true;
        yield return new WaitForSeconds(2 - 1/fireRate);
        if (bullet != null)
            Destroy(bullet);
    }
}