/* (Even though there are a lot of similarities between this and the player 
controller, I don't feel like doing any abstraction cause it would be too much effort - kevin) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Projectile Firing")]
    [SerializeField] private float fireRate;
    [SerializeField] private float attackRadius;
    public GameObject player;
    public GameObject projectile;

    [Header("Enemy Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityCoefficient; // If the enemy is in a reverse gravity area 

    [Header("Collision Checking - Layer Masks")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Dying")]
    public GameObject deathEffect; 

    [Header("Indicator")]
    public GameObject angerVein;


    private PlayerController playerController;
    private Rigidbody2D rb2d;
    [HideInInspector] public Rigidbody2D player_rb2d;
    private BoxCollider2D bc2d;
    private bool canShoot;
    private bool isGrounded;

    private bool targetDetected;

    private float projectileMoveSpeed;

    public float ProjectileMoveSpeed {
        get { return projectileMoveSpeed;}
        set { projectileMoveSpeed = value;}
    }
    public float distanceFromPlayer;
    public float timeFromPlayer;
    public Vector2 futurePlayerPosition;
    public Vector2 projectileDirection;

    public bool obstructedLineOfSight;

    // private bool canCollide = true;
    // public bool CanCollide {
    //     get{return canCollide;}
    //     set{canCollide = value;}
    // }

    // Why do we need canCollide here - Kevin

    private float direction;

    // Start is called before the first frame update
    void Start()
    {
        // canCollide = true;
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        player_rb2d = player.GetComponent<Rigidbody2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        canShoot = true;

        direction = 1f;
    }
    // Called every fixed timestep
    // Used for physics
    void FixedUpdate()
    {
        // Temporarily disable the shooting so that I can easily work on the movement - kevin
        // Temporarily enable the shooting so that I can easily work on the aim - TJ
        if(isGrounded && obstructedLineOfSight)
            rb2d.velocity = new Vector2(direction * moveSpeed * 100f * Time.fixedDeltaTime, rb2d.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        
        Ray2D ray2D = new Ray2D(transform.position, projectileDirection);
        Debug.DrawRay(transform.position, ray2D.direction * attackRadius);

        obstructedLineOfSight = !Physics2D.Raycast(ray2D.origin, projectileDirection, attackRadius, whatIsPlayer);
        if (!obstructedLineOfSight) angerVein.SetActive(true);
        else angerVein.SetActive(false);
        
        if(distanceFromPlayer <= attackRadius && canShoot && !obstructedLineOfSight){
            StartCoroutine(Shoot());
        }

        bool collideWithPlayer = Physics2D.BoxCast(transform.position, new Vector2(0.45f, 0.45f), 0f, Vector2.down, 0f, whatIsPlayer);
        isGrounded = Physics2D.BoxCast(transform.position, new Vector2(0.45f - 0.1f, 0.45f - 0.1f), 0f, gravityCoefficient * Vector2.down, 0.1f, whatIsGround);

        if(collideWithPlayer)
        {
            if(playerController.IsDashing)
            {
                Die();
            }
        }
    }

    void Die()
    {
        GameObject deathParticles = Instantiate(deathEffect);   
        deathParticles.transform.position = transform.position;

        StopAllCoroutines();
        gameObject.SetActive(false);

        ParticleSystem dpSystem = deathParticles.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule dpMain = dpSystem.main;

        dpMain.startColor = gameObject.GetComponent<SpriteRenderer>().color;

        Destroy(deathParticles, 2f);
    }


    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Kill")
        {
            Die();
        }

        if(col.gameObject.tag == "DirectionChange")
        {
            direction *= -1;
        }

    }

    // void respawn(){
    //     gameObject.SetActive(true);
    //     transform.position = spawnPoint;
    // }  

    // Commented this out since I don't think enemies should respawn - kevin

    IEnumerator Shoot()
    {
        canShoot = false;
        GameObject Projectile = Instantiate(projectile) as GameObject;
        Rigidbody2D projectileRb2d = Projectile.GetComponent<Rigidbody2D>();
        Projectile projectileScript = Projectile.GetComponent<Projectile>();
        Projectile.SetActive(true);
        projectileScript.shoot();
        yield return new WaitForSeconds(1/fireRate);
        canShoot = true;
        yield return new WaitForSeconds(2 - 1/fireRate);
        if (Projectile != null){
            Destroy(gameObject);
        }
    }
}