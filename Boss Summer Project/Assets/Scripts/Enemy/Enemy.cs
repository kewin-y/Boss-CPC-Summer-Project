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
    [SerializeField] private float projectileMoveSpeed;

    [Header("Size")]
    [SerializeField] private Vector2 enemySize;
    private GameObject player;
    public GameObject projectile;

    [Header("Enemy Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityCoefficient; // If the enemy is in a reverse gravity area 

    [Header("Collision Checking - Layer Masks")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsCamera;
    [SerializeField] private LayerMask directionChangeLayerMask;
    [SerializeField] private LayerMask excludeColliders;

    [Header("Dying")]
    public GameObject deathEffect;
    [SerializeField] private Color deathColour;

    [Header("Indicator")]
    [SerializeField] private SpriteRenderer glasses;
    [SerializeField] private Color glassesAngryColor;
    [SerializeField] private Color glassesHappyColor;

    [Header("Dash Immunity")]
    [SerializeField] private bool isImmune;

    [Header("Player Weapons")]
    [SerializeField] private GameObject playerSword;

    private PlayerController playerController;
    private Rigidbody2D rb2d;
    private Rigidbody2D player_rb2d;
    private bool canShoot;
    private bool isGrounded;

    private float distanceFromPlayer;
    private float timeFromPlayer;
    private Vector2 futurePlayerPosition;
    private Vector2 projectileDirection;

    private bool obstructedLineOfSight;

    private bool isInCamera;
    private float direction;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        player_rb2d = player.GetComponent<Rigidbody2D>();

        playerController = player.GetComponent<PlayerController>();
        canShoot = true;

        direction = 1f;
    }

    void FixedUpdate()
    {
        if (isGrounded && obstructedLineOfSight && isInCamera)
            rb2d.velocity = new Vector2(direction * moveSpeed * 100f * Time.fixedDeltaTime, rb2d.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        isInCamera = Physics2D.BoxCast(transform.position, new Vector2(0.45f, 0.45f), 0f, Vector2.down, 0f, whatIsCamera);

        // This is clever
        distanceFromPlayer = (transform.position - player.transform.position).magnitude;
        timeFromPlayer = distanceFromPlayer / projectileMoveSpeed;
        futurePlayerPosition = (Vector2)player.transform.position + timeFromPlayer * player_rb2d.velocity;
        projectileDirection = (futurePlayerPosition - (Vector2)transform.position).normalized;

        RaycastHit2D raycastHit2D = Physics2D.CircleCast(transform.position, 0.12f, projectileDirection, attackRadius - 0.12f, excludeColliders);

        obstructedLineOfSight = !raycastHit2D || !raycastHit2D.collider.CompareTag("Player");

        if (!obstructedLineOfSight)
        {
            direction = (player.transform.position.x - transform.position.x) / Mathf.Abs(player.transform.position.x - transform.position.x);
            glasses.color = glassesAngryColor;

            if (!isRunning) StartCoroutine(UntilObstructed());
        }

        else
        {
            glasses.color = glassesHappyColor;
        }


        if (distanceFromPlayer <= attackRadius && canShoot && !obstructedLineOfSight && isInCamera)
        {
            StartCoroutine(Shoot());
        }

        bool collideWithPlayer = Physics2D.OverlapBox(transform.position, new Vector2(0.45f, 0.45f), 0, whatIsPlayer);
        isGrounded = Physics2D.BoxCast(transform.position, enemySize, 0f, gravityCoefficient * Vector2.down, 0.1f, whatIsGround);

        if (collideWithPlayer)
        {
            if (playerController.IsDashing && (!isImmune || playerController.SwordHeld))
            {
                if (playerController.SwordHeld)
                {
                    playerController.SwordHeld = false;
                    player.transform.Find("Sword").gameObject.SetActive(false);
                }


                Die();
            }
        }

        Debug.DrawRay(transform.position, new Vector2(direction * -1, 0) * 0.3f);
        transform.localScale = new(direction, 1f, 1f);

    }

    IEnumerator UntilObstructed()
    {
        isRunning = true;
        yield return new WaitUntil(() => obstructedLineOfSight);

        isRunning = false;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, new(direction * -1, 0), 0.3f, directionChangeLayerMask);

        if (!raycastHit2D)
        {
            direction *= -1;
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

        dpMain.startColor = deathColour;

        Destroy(deathParticles, 2f);
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Shield"))
            return;

        else if (col.gameObject.CompareTag("Kill"))
            Die();

        else if (col.gameObject.tag == "DirectionChange")
            direction *= -1;

    }

    //KEVIN'S JOB
    // Thanks
    public void Respawn()
    {
        gameObject.SetActive(true);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        GameObject projectileObject = Instantiate(projectile);
        Rigidbody2D projectileRb2d = projectileObject.GetComponent<Rigidbody2D>();
        projectileObject.SetActive(true);

        float angle = 0;
        float refAngle = Mathf.Atan(projectileDirection.y / projectileDirection.x) * Mathf.Rad2Deg;

        if (projectileDirection.y > 0 && projectileDirection.x > 0)
        {
            angle = refAngle;
        }

        else if (projectileDirection.y < 0 && projectileDirection.x > 0)
        {
            angle = 360 + refAngle;
        }

        else if (projectileDirection.y > 0 && projectileDirection.x < 0)
        {
            angle = 180 + refAngle;
        }

        else if (projectileDirection.y < 0 && projectileDirection.x < 0)
        {
            angle = 180 + refAngle;
        }

        else if (projectileDirection.y == 0)
        {

            if (projectileDirection.x > 0)
            {
                angle = 0;
            }
            else
            {
                angle = 180;
            }
        }

        else if (projectileDirection.x == 0)
        {
            if (projectileDirection.y > 0)
            {
                angle = 90;
            }
            else
            {
                angle = 270;
            }
        }

        Quaternion orientation = Quaternion.Euler(0, 0, angle);
        projectileObject.transform.SetPositionAndRotation((Vector2)transform.position, orientation);
        projectileRb2d.velocity = projectileMoveSpeed * Time.fixedDeltaTime * projectileDirection;

        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
        yield return new WaitForSeconds(2 - 1 / fireRate);

        if (projectileObject != null)
        {
            Destroy(projectileObject);
        }
    }
}
