using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Damageable
{
    private enum TerrainState
    {
        Air,
        Water
    }
    private TerrainState terrainState;

    //Inspector input
    [SerializeField] private float dashForce;
    [SerializeField] private float jumpHeight;

    public int jumpsRemaining;
    public int jumpsAvailable;

    [SerializeField] private float moveSpeed;

    public float MoveSpeed {
        get {return moveSpeed;}
        set {moveSpeed = value;}
    }

    [SerializeField] private float jumpVelocity;
    [SerializeField] private float coyoteTime; // Time between last grounded where the player can still jump midair; makes controller more fair and responsive
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWater;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float dashAmount;
    [SerializeField] private float dashCooldown;
    [SerializeField] private Color playerColor; // For death particles mainly
    [SerializeField] private GameObject shield;

    public Camera mainCam;
    private CameraBounds cameraBounds;
    public GameObject deathEffect;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private float xInput; // Variable for the x-input (a&d or left & right)
    private bool isGrounded; // If the player is on the ground 
    private bool isInWater;
    private float lastGrounded; // Or airtime; time since the player was last grounded
    private bool canJump;
    private bool isJumping;
    private bool wishJump; // Jump queueing; no holding down the button to jump repeatedly, but pressing before the player is grouded will make the square jump as soon as it lands
    private float playerSize; // Appears to be 0.5 but hitbox (box collider) is slightly smaller to make it more fair
    private float actualPlayerSize; // Disregards the "slightly smaller" hitbox for playerSize
    private bool canDash;
    private bool isDashing;

    [SerializeField] private HealthBar healthBar;

    public bool IsDashing
    {
        get {return isDashing;}
        set {isDashing = value;}
    }
    private float lastFacing;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraBounds = mainCam.GetComponent<CameraBounds>();

        actualPlayerSize = transform.localScale.x;
        playerSize = actualPlayerSize - 0.05f;
        lastGrounded = 0f;
        canJump = true;
        canDash = true;
        lastFacing = 1;
        jumpsRemaining = jumpsAvailable = 2;

        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize), 0f, Vector2.down, 0.1f, whatIsGround);
        isInWater = Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize - 0.1f), 0f, Vector2.down, 0f, whatIsWater);

        if(isInWater)
            terrainState = TerrainState.Water;
        else
            terrainState = TerrainState.Air;

        if(!isGrounded)
            lastGrounded += Time.deltaTime;

        else
        {
            isJumping = false;
            lastGrounded = 0f;
        }
        
        getInput();

        transform.localScale = new Vector3(lastFacing * actualPlayerSize, actualPlayerSize, actualPlayerSize);
    }

    void getInput()
    { 
        xInput = Input.GetAxisRaw("Horizontal");
        if(xInput != 0) lastFacing = xInput;

        if(Input.GetKeyDown(jumpKey) && !wishJump) wishJump = true; // Player can queue a jump as long as the jump key (SAPCE) is held
        if(Input.GetKeyUp(jumpKey)) wishJump = false;

        if(Input.GetKeyDown(dashKey) && canDash)
        {
            StartCoroutine(dash());
        }

        if(terrainState == TerrainState.Air) // Lol change this to a switch statement later
        {
            rb2d.gravityScale = 5f;
            rb2d.drag = 0f;

            if(wishJump && canJump && jumpsRemaining > 0) 
            {
                jump(); 
                wishJump = false;
                canJump = false; // canJump variable to prevent accidental double-jumping due to coyote time; implement a double-jumping mechanism that isn't actually a bug
                jumpsRemaining -= 1;
                if (lastGrounded < coyoteTime || isJumping){
                    isJumping = true;
                    jump();
                }

                Invoke("resetJump", coyoteTime + 0.1f); // Resets the jump after the coyote time period
            }
        }

        else if (terrainState == TerrainState.Water)
        {
            rb2d.gravityScale = 1f;
            rb2d.drag = 1f;

            if(wishJump)
            {
                jump();
            }
        }  
    }

    void resetJump() => canJump = true; // This is syntax for a one-line method

    void jump()
    {
        if(terrainState == TerrainState.Air)
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);

        else if (terrainState == TerrainState.Water)
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity * 0.25f);
    }

    IEnumerator dash()
    {
        
        isDashing = true;
        canDash = false;

        rb2d.gravityScale = 0f;

        Vector2 distance = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 direction = distance.normalized;

        rb2d.AddForce(direction * dashAmount, ForceMode2D.Impulse);

        lastFacing = direction.x / Mathf.Abs(direction.x);

        yield return new WaitForSeconds(0.2f);

        float afterDashVelo = direction.x / Mathf.Abs(direction.x) * moveSpeed;

        for(float t = 0.0f; t < 1f; t += Time.deltaTime / 0.1f)
        {
            rb2d.velocity = new Vector2(Mathf.Lerp(rb2d.velocity.x, afterDashVelo, t), Mathf.Lerp(rb2d.velocity.y, 0, t));
            yield return null;
        }

        isDashing = false;
        rb2d.gravityScale = 5f;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        // TODO: make a dash animation where the square turns into some sort of wind thing and follows the direction of the dash
    }
    
    void FixedUpdate()
    {
        if(isDashing)
            return;
        
        rb2d.velocity = new Vector2(xInput * moveSpeed * 100f * Time.fixedDeltaTime, rb2d.velocity.y);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        bool isCollidingWithWall = Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize - 0.1f), 0f, Vector2.left, 0f, whatIsGround) || Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize - 0.1f), 0f, Vector2.right, 0f, whatIsGround);
        print(isCollidingWithWall);
        
        if(col.gameObject.layer == 6 && gameObject.transform.position[1] - col.GetContact(0).point[1] > 0 && (isGrounded || isCollidingWithWall)){
            jumpsRemaining = jumpsAvailable;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Kill")
        {   
            Die();
        }
    }

    public override void Die()
    {
        cameraBounds.CameraCanMove = false;
        StopAllCoroutines();
        gameObject.SetActive(false); 

        shield.SetActive(false);

        GameObject deathParticles = Instantiate(deathEffect);
        deathParticles.transform.position = transform.position;

        ParticleSystem dpSystem = deathParticles.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule dpMain = dpSystem.main;

        dpMain.startColor = playerColor;

        Destroy(deathParticles, 2f);

        Invoke("respawn", 1f);
    }

    void respawn() 
    {
        health = maxHealth;
        healthBar.SetHealth(health);
        
        canDash = true;
        rb2d.gravityScale = 5f;
        isDashing = false;
        gameObject.SetActive(true);
        transform.position = spawnPoint; // tp the player to the spawnpoint
        mainCam.transform.position = new Vector3(0, 0, -10f);
        cameraBounds.CameraCanMove = true;

        VisualEffects.SetColor(gameObject, Color.white);

    }

    //Depletes the player's health by a certain amount
    public override void TakeDamage(int damage) {
        health -= damage;
        healthBar.SetHealth(health);

        VisualEffects.SetColor(gameObject, Color.red);
        StartCoroutine(VisualEffects.FadeToColor(gameObject, 0.5f, Color.white));

        if(health <= 0)
            Die();
    }
}
