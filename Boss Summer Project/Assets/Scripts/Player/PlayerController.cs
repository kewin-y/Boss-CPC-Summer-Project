using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Damageable
{
    private enum TerrainState
    {
        Air,
        Water,
        Ice
    }
    private TerrainState terrainState;

    [HideInInspector] public int jumpsRemaining;
    [HideInInspector] public int jumpsAvailable;
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
    [SerializeField] private LayerMask whatIsIce;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float dashAmount;
    [SerializeField] private float dashCooldown;
    [SerializeField] private Color playerColor; // For death particles mainly
    [SerializeField] private GameObject shield;

    public Camera mainCam;
    private CameraBounds cameraBounds;
    public GameObject deathEffect;
    public ParticleSystem waterParticles;

    private BoxCollider2D bc2d;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private float xInput; // Variable for the x-input (a&d or left & right)
    private bool isGrounded; // If the player is on the ground 
    private bool isInWater;
    private bool isOnIce;
    private float lastGrounded; // Or airtime; time since the player was last grounded
    private bool canJump;
    private bool doubleJump;
    private bool wishJump; // Jump queueing; no holding down the button to jump repeatedly, but pressing before the player is grouded will make the square jump as soon as it lands
    private float playerSize = 0.45f; // Appears to be 0.5 but hitbox (box collider) is slightly smaller to make it more fair
    private float actualPlayerSize; // Disregards the "slightly smaller" hitbox for playerSize
    private bool canDash;
    private bool isDashing;
    private int gravityCoefficient = 1; //If 1, gravity goes down. If -1, gravity goes up.
    public int GravityCoefficient {
        get { return gravityCoefficient; }
        set { gravityCoefficient = value; }
    }

    [SerializeField] private HealthBar regularHealthBar;
    [SerializeField] private HealthBar extraHealthBar;

    public bool IsDashing
    {
        get {return isDashing;}
        set {isDashing = value;}
    }
    private float lastFacing = 1;   //If 1, facing right. If -1, facing left.
    private float horizontalFlip = 1;    //If -1, player is upside down; must flip horizontally.
    public UnityEvent respawnEvent; //Called when the player respawns

    [SerializeField] private Transform powerUps;    //Parent object for all power ups
    [SerializeField] private Transform items;       //Parent object for all items

    private bool isFlipped;
    public bool IsFlipped {
        get { return isFlipped; }
        set { isFlipped = value; }
    }

    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Sprite ninjaSprite;
    [SerializeField] private GameObject ninjaAttachment;

    // Dawg we gotta organize these fields; w/ headers, regions, etc. - kevin
    // Let's do it on a call - Sean

    // Start is called before the first frame update
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraBounds = mainCam.GetComponent<CameraBounds>();

        actualPlayerSize = transform.localScale.x;
        lastGrounded = 0f;
        jumpsRemaining = jumpsAvailable = 2;

        SetupRespawnEvent();
        Respawn();
    }

    //Add all power ups and items as listeners for the respawn event
    //NOTE: This makes it unnecessary to do so in the inspector.
    private void SetupRespawnEvent() {

        //Add power ups as listeners
        for (int i = 0; i < powerUps.childCount; i++) {
            PowerUp powerUpScript = powerUps.GetChild(i).GetComponent<PowerUp>();
            respawnEvent.AddListener(powerUpScript.Respawn);
        }

        //Add items as listeners
        for (int i = 0; i < items.childCount; i++) {
            Item itemScript = items.GetChild(i).GetComponent<Item>();
            respawnEvent.AddListener(itemScript.Respawn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.down, 0.1f, whatIsGround);
        isInWater = Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.down, 0f, whatIsWater);
        isOnIce = Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.down, 0.1f, whatIsIce);

        if(isInWater)
            terrainState = TerrainState.Water;
        else if (isOnIce)
            terrainState = TerrainState.Ice;
        else
            terrainState = TerrainState.Air;

        if(!isGrounded)
            lastGrounded += Time.deltaTime;
        else
            lastGrounded = 0f;
        
        getInput();

        transform.localScale = new Vector3(horizontalFlip * lastFacing * actualPlayerSize, actualPlayerSize, actualPlayerSize);
    }

    void getInput()
    { 
        xInput = Input.GetAxisRaw("Horizontal");
        if(xInput != 0)
        {
            if(terrainState == TerrainState.Water)
                if(!waterParticles.isPlaying) waterParticles.Play();
                
            lastFacing = xInput;
        }
         

        if(Input.GetKeyDown(jumpKey) && !wishJump) wishJump = true; // Player can queue a jump as long as the jump key (SPACE) is held
        if(Input.GetKeyUp(jumpKey)) wishJump = false;

        if(Input.GetKeyDown(dashKey) && canDash)
            StartCoroutine(dash());

        //Jump behaviours + gravity and drag
        if(terrainState == TerrainState.Air || terrainState == TerrainState.Ice) // Lol change this to a switch statement later
        {
            if(waterParticles.isPlaying) waterParticles.Stop();

            SetGravityScale(5f);
            rb2d.drag = 0f;

            if(wishJump && canJump && (lastGrounded < coyoteTime || doubleJump) && jumpsRemaining > 0)
            {
                jump(); 
                wishJump = false;
                canJump = false; // canJump variable to prevent accidental double-jumping due to coyote time; implement a double-jumping mechanism that isn't actually a bug
                doubleJump = true;
                jumpsRemaining -= 1;

                Invoke("resetJump", coyoteTime + 0.1f); // Resets the jump after the coyote time period
            }
        }

        else if (terrainState == TerrainState.Water)
        {

            SetGravityScale(1f);
            rb2d.drag = 1f;

            if(wishJump)
            {
                jump();

                if(!waterParticles.isPlaying) waterParticles.Play();
            }

            else if (!wishJump && xInput == 0)
            {
                if(waterParticles.isPlaying) waterParticles.Stop();
            }
        }

        
    }

    public void flipHorizontal() {
        horizontalFlip *= -1;
    }

    void resetJump() => canJump = true; // This is syntax for a one-line method

    void jump()
    {
        if(terrainState == TerrainState.Air || terrainState == TerrainState.Ice)
            rb2d.velocity = new Vector2(rb2d.velocity.x, gravityCoefficient * jumpVelocity);

        else if (terrainState == TerrainState.Water)
            rb2d.velocity = new Vector2(rb2d.velocity.x, gravityCoefficient * jumpVelocity * 0.25f);
    }

    IEnumerator dash()
    {
        
        isDashing = true;
        canDash = false;

        SetGravityScale(0f);

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
        SetGravityScale(5f);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        // TODO: make a dash animation where the square turns into some sort of wind thing and follows the direction of the dash
    }
    
    void FixedUpdate()
    {
        if(isDashing)
            return;
        
        //To keep things consistent with friction, we should use AddForce even for normal movement - Sean
        if (terrainState == TerrainState.Ice) {
            print("ice");
            rb2d.AddForce(new Vector2(xInput * moveSpeed * 100f * Time.fixedDeltaTime, rb2d.velocity.y));
        }
        else
            rb2d.velocity = new Vector2(xInput * moveSpeed * 100f * Time.fixedDeltaTime, rb2d.velocity.y);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Kill")
            Die();

        else if (IsInLayerMask(col.gameObject, whatIsGround)) {
            bool touchingGround = Physics2D.BoxCast(transform.position, new Vector2(playerSize, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.down, 0.1f, whatIsGround);
            if (touchingGround) {
                jumpsRemaining = jumpsAvailable;
                doubleJump = false;
            }
        }
    } // I jus realized why is there a touchingGround variable and an isGrounded variable - kevin

    /*
    This method checks if a GameObject belongs in a layer mask.
    Since a layer mask in Unity is simply a binary string where each digit
    represents a layer (e.g. 000000001 means layer 0 is included in the mask),
    we can use bit shifting to check if the object's layer matches any of the layers
    in the layer mask.
    */
    private bool IsInLayerMask(GameObject obj, LayerMask layerMask) {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
    
    public override void Die()
    {
        cameraBounds.CameraCanMove = false;
        StopAllCoroutines();

        spriteRenderer.enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        shield.SetActive(false);

        GameObject deathParticles = Instantiate(deathEffect);
        deathParticles.transform.position = transform.position;

        ParticleSystem dpSystem = deathParticles.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule dpMain = dpSystem.main;

        dpMain.startColor = playerColor;

        Destroy(deathParticles, 2f);
        Invoke("InvokeRespawnEvent", 1f);
    }

    private void InvokeRespawnEvent() => respawnEvent.Invoke();

    //Called by respawn UnityEvent; resets all player settings
    public void Respawn() 
    {
        health = maxHealth;
        regularHealthBar.SetHealth(health);
        absorptionHealth = 0.0f;
        extraHealthBar.SetHealth(absorptionHealth);

        spriteRenderer.enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        
        gravityCoefficient = 1;
        isFlipped = false;
        transform.eulerAngles = Vector3.zero;
        SetGravityScale(5f);
        rb2d.velocity = Vector2.zero;

        canJump = true;
        canDash = true;
        isDashing = false;
        transform.position = spawnPoint; // tp the player to the spawnpoint
        mainCam.transform.position = new Vector3(0, 0, -10f);
        cameraBounds.CameraCanMove = true;
        wishJump = false;

        VisualEffects.SetColor(gameObject, Color.white);
        SwitchToDefaultCostume();
    } 

    //Depletes the player's health by a certain amount
    public override void TakeDamage(float damage) {
        if (absorptionHealth > damage) {
            absorptionHealth -= damage;
            extraHealthBar.SetHealth(absorptionHealth);
        } 
        else if (absorptionHealth > 0){
            health -= damage - absorptionHealth;
            regularHealthBar.SetHealth(health);
            absorptionHealth = 0;
            extraHealthBar.SetHealth(absorptionHealth);
        } 
        else {
            health -= damage;
            regularHealthBar.SetHealth(health);
        }

        VisualEffects.SetColor(gameObject, Color.red);
        if(isActiveAndEnabled) StartCoroutine(VisualEffects.FadeToColor(gameObject, 0.5f, Color.white));

        if(health <= 0)
            Die();
    }

    public override void Heal(int healAmount) {
        health += healAmount;
        
        //Health cannot overflow
        if (health > maxHealth)
            health = maxHealth;

        regularHealthBar.SetHealth(health);
    }

    //Sets the player's gravity scale while taking the gravity coefficient into account
    private void SetGravityScale(float newGravityScale) {
        rb2d.gravityScale = gravityCoefficient * newGravityScale;
    }

    //Switches the player's costume to the default costume
    public void SwitchToDefaultCostume() {
        spriteRenderer.sprite = playerSprite;
        ninjaAttachment.SetActive(false);
    }

    //Switches the player's costume to a ninja costume
    public void SwitchToNinjaCostume() {
        spriteRenderer.sprite = ninjaSprite;
        ninjaAttachment.SetActive(true);
    }

}
