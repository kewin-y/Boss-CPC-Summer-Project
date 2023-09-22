using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : Damageable
{
    #region Inspector Values
    [Header("Jumping")]
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float coyoteTime; // Time between last grounded where the player can still jump midair; makes controller more fair and responsive

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    [Header("Dashing")]
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private float dashAmount;
    [SerializeField] private float dashCooldown;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWater;
    [SerializeField] private LayerMask whatIsIce;
    [SerializeField] private LayerMask whatIsMud;

    [Header("Locations")]
    [SerializeField] private Vector3 spawnPoint;

    [Header("Events")]
    public UnityEvent playerJumped;

    [Header("Object References")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private ParticleSystem waterParticles;
    [SerializeField] private GameObject shield;
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Sprite ninjaSprite;
    [SerializeField] private GameObject ninjaAttachment;
    [SerializeField] private HealthBar regularHealthBar;
    [SerializeField] private HealthBar extraHealthBar;
    [SerializeField] private DashMeter dashMeter;

    [Header("Weapons")]
    [SerializeField] private GameObject sword;

    [Header("Miscellaneous")]
    [SerializeField] private Color playerColor; // For death particles mainly
    #endregion

    #region Terrain
    private enum TerrainState
    {
        Air,
        Water,
        Ice,
        Mud
    }

    private TerrainState terrainState;
    #endregion

    #region Jumping
    private int jumpsRemaining;
    public int JumpsRemaining
    {
        get { return jumpsRemaining; }
        set { jumpsRemaining = value; }
    }

    private int jumpsAvailable = 2;
    public int JumpsAvailable
    {
        get { return jumpsAvailable; }
        set { jumpsAvailable = value; }
    }

    private bool isGrounded; // If the player is on the ground
    private float lastGrounded; // Or airtime; time since the player was last grounded
    private bool canJump;
    private bool doubleJump;
    private bool wishJump; // Jump queueing; no holding down the button to jump repeatedly, but pressing before the player is grouded will make the square jump as soon as it lands
    #endregion

    #region Camera
    private CameraBounds cameraBounds;
    #endregion

    #region Player Information
    private BoxCollider2D bc2d;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private float playerSize = 0.45f; // Appears to be 0.5 but hitbox (box collider) is slightly smaller to make it more fair
    private float actualPlayerSize; // Disregards the "slightly smaller" hitbox for playerSize
    private float lastFacing = 1;       //If 1, facing right. If -1, facing left.
    private float horizontalFlip = 1;   //If -1, player is upside down; must flip horizontally.
    #endregion

    #region Movement
    private float xInput; // Variable for the x-input (a&d or left & right)

    private int gravityCoefficient = 1; //If 1, gravity goes down. If -1, gravity goes up.
    public int GravityCoefficient
    {
        get { return gravityCoefficient; }
        set { gravityCoefficient = value; }
    }

    private bool isFlipped;
    public bool IsFlipped
    {
        get { return isFlipped; }
        set { isFlipped = value; }
    }
    #endregion

    #region Dashing
    private bool canDash;

    private bool isDashing;
    public bool IsDashing
    {
        get { return isDashing; }
        set { isDashing = value; }
    }


    #endregion

    #region Statistics
    private GameObject statistics;
    private StatisticsSystem statisticsScript;
    #endregion

    #region Inventory
    private int ironOwned;
    public int IronOwned
    {
        get { return ironOwned; }
        set { ironOwned = value; }
    }

    private int spikyBlocksOwned;
    public int SpikyBlocksOwned
    {
        get { return spikyBlocksOwned; }
        set { spikyBlocksOwned = value; }
    }

    private int swordOwned;
    public int SwordOwned
    {
        get { return swordOwned; }
        set { swordOwned = value; }
    }

    private int batteryOwned;
    public int BatteryOwned
    {
        get { return batteryOwned; }
        set { batteryOwned = value; }
    }

    private int batteryBlockOwned;
    public int BatteryBlockOwned
    {
        get { return batteryBlockOwned; }
        set { batteryBlockOwned = value; }
    }

    [SerializeField] private KeyCode placeSpikyBlockKey;
    [SerializeField] private KeyCode placeBatteryBlockKey;
    [SerializeField] private KeyCode equipSwordKey;
    [SerializeField] private GameObject spikyBlock;
    [SerializeField] private GameObject batteryBlock;
    [SerializeField] private float placementRange;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // statistics = GameObject.Find("Statistics");
        // statisticsScript = statistics.GetComponent<StatisticsSystem>();
        // playerJumped.AddListener(statisticsScript.AddJump);

        // GameObject[] gameObjects = FindObjectsOfType<GameObject>();

        // foreach (GameObject element in gameObjects) {
        //     if (element.scene.name == "Statistics Menu" && element.name == "Canvas" && element.layer == 5) {
        //         element.SetActive(false);
        //     }
        // }

        bc2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraBounds = mainCam.GetComponent<CameraBounds>();

        actualPlayerSize = transform.localScale.x;
        lastGrounded = 0f;
        jumpsRemaining = jumpsAvailable;

        InvokeRespawnEvent();
    }

    // Update is called once per frame
    void Update()
    {
        TerrainCheck();
        SpeedControl();

        if (!isGrounded)
            lastGrounded += Time.deltaTime;
        else
        {
            lastGrounded = 0f;
        }

        if (!PauseManager.IsPaused)
            getInput();

        transform.localScale = new Vector3(horizontalFlip * lastFacing * actualPlayerSize, actualPlayerSize, actualPlayerSize);

        StatisticsSystem.DistanceTraveled += (rb2d.velocity * Time.deltaTime).magnitude;
    }

    void TerrainCheck()
    {
        bool isInWater = Physics2D.BoxCast(transform.position, new Vector2(playerSize, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.down, 0f, whatIsWater);
        bool isOnIce = Physics2D.BoxCast(transform.position, new Vector2(playerSize, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.down, 0.1f, whatIsIce);
        bool isOnMud = Physics2D.BoxCast(transform.position, new Vector2(playerSize, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.down, 0.1f, whatIsMud);

        if (isInWater)
            terrainState = TerrainState.Water;
        else if (isOnIce)
            terrainState = TerrainState.Ice;
        else if (isOnMud)
            terrainState = TerrainState.Mud;
        else
            terrainState = TerrainState.Air;
    }

    void SpeedControl()
    {
        if (isDashing)
            return;

        Vector2 flatVelocity = new(rb2d.velocity.x, 0f);

        if (terrainState == TerrainState.Ice)
        {
            if (flatVelocity.magnitude > moveSpeed * 1.1f)
            {
                Vector2 controlledSpeed = flatVelocity.normalized * (moveSpeed * 1.1f);
                rb2d.velocity = new Vector2(controlledSpeed.x, rb2d.velocity.y);
            }
        }

        else if (terrainState == TerrainState.Mud)
        {
            if (flatVelocity.magnitude > moveSpeed * 0.15f)
            {
                Vector2 controlledSpeed = flatVelocity.normalized * (moveSpeed * 0.15f);
                rb2d.velocity = new Vector2(controlledSpeed.x, rb2d.velocity.y);
            }
        }

        else
        {
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector2 controlledSpeed = flatVelocity.normalized * (moveSpeed * 0.5f);
                rb2d.velocity = new Vector2(controlledSpeed.x, rb2d.velocity.y);
            }
        }

    }

    void getInput()
    {
        if (Input.GetKeyDown(equipSwordKey) && swordOwned >= 1) {
            swordOwned -= 1;
            sword.SetActive(true);
        }

        Vector2 worldMousePosition = (Vector2) mainCam.ScreenToWorldPoint(Input.mousePosition);
        bool canPlaceBlock = !Physics2D.OverlapBox(worldMousePosition, new Vector2(0.1f, 0.1f), 0f) && ((worldMousePosition - (Vector2)transform.position).magnitude <= placementRange);

        if (Input.GetKeyDown(placeSpikyBlockKey) && canPlaceBlock)
        {
            GameObject newSpikyBlock = Instantiate(spikyBlock) as GameObject;
            newSpikyBlock.transform.position = worldMousePosition;
        } 
        else if (Input.GetKeyDown(placeBatteryBlockKey) && canPlaceBlock)
        {
            GameObject newBatteryBlock = Instantiate(batteryBlock) as GameObject;
            newBatteryBlock.transform.position = worldMousePosition;
        }

        xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0)
        {
            if (terrainState == TerrainState.Water)
                if (!waterParticles.isPlaying) waterParticles.Play();

            lastFacing = xInput;
        }
        else if (!isDashing && terrainState != TerrainState.Ice)
            rb2d.velocity = new(0, rb2d.velocity.y);

        if (Input.GetKeyDown(jumpKey) && !wishJump) wishJump = true; // Player can queue a jump as long as the jump key (SPACE) is held
        if (Input.GetKeyUp(jumpKey)) wishJump = false;

        if (Input.GetKeyDown(dashKey) && canDash)
            StartCoroutine(dash());

        //Jump behaviours + gravity and drag
        if (terrainState != TerrainState.Water) // Lol change this to a switch statement later
        {
            if (waterParticles.isPlaying) waterParticles.Stop();

            SetGravityScale(5f);
            rb2d.drag = 0f;

            if (wishJump && canJump && (lastGrounded < coyoteTime || doubleJump) && jumpsRemaining > 0)
            {
                jump();
                wishJump = false;
                canJump = false; // canJump variable to prevent accidental double-jumping due to coyote time; implement a double-jumping mechanism that isn't actually a bug
                doubleJump = true;

                if (!(lastGrounded < coyoteTime))
                    jumpsRemaining -= 1;

                Invoke("resetJump", coyoteTime + 0.1f); // Resets the jump after the coyote time period
            }
        }

        else
        {

            SetGravityScale(1f);
            rb2d.drag = 1f;

            if (wishJump)
            {
                jump();

                if (!waterParticles.isPlaying) waterParticles.Play();
            }

            else if (!wishJump && xInput == 0)
            {
                if (waterParticles.isPlaying) waterParticles.Stop();
            }
        }


    }

    public void flipHorizontal()
    {
        horizontalFlip *= -1;
    }

    void resetJump() => canJump = true;

    void jump()
    {
        if (terrainState != TerrainState.Water)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, gravityCoefficient * jumpVelocity);
            playerJumped.Invoke();
        }
        else
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, gravityCoefficient * jumpVelocity * 0.25f);
        }
    }

    IEnumerator dash()
    {
        dashMeter.StartCoroutine(dashMeter.StartSequence(dashCooldown + 0.3f));

        isDashing = true;
        canDash = false;

        SetGravityScale(0f);

        Vector2 distance = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 direction = distance.normalized;

        rb2d.AddForce(direction * dashAmount, ForceMode2D.Impulse);

        lastFacing = direction.x / Mathf.Abs(direction.x);

        yield return new WaitForSeconds(0.2f);

        float afterDashVelo = direction.x / Mathf.Abs(direction.x) * moveSpeed;

        for (float t = 0.0f; t < 1f; t += Time.deltaTime / 0.1f)
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
        if (isDashing)
            return;

        if (terrainState == TerrainState.Ice)
            rb2d.AddForce(new Vector2(xInput * moveSpeed * 30f * Time.fixedDeltaTime, 0f));
        else
            rb2d.AddForce(new Vector2(xInput * moveSpeed * 100f * Time.fixedDeltaTime, 0f));
    }

    void OnCollisionStay2D(Collision2D col)
    {

        if (col.gameObject.tag == "Kill")
            Die();

        else if (IsInLayerMask(col.gameObject, whatIsGround))
        {
            isGrounded = Physics2D.BoxCast(transform.position, new Vector2(playerSize, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.down, 0.1f, whatIsGround);

            if (isGrounded)
            {
                doubleJump = false;
                jumpsRemaining = jumpsAvailable;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {

        bool headHitter = Physics2D.BoxCast(transform.position, new Vector2(playerSize, playerSize - 0.1f), 0f, gravityCoefficient * Vector2.up, 0.2f, whatIsGround);

        if (IsInLayerMask(col.gameObject, whatIsGround))
        {
            isGrounded = false;
            doubleJump = true;

            if (!headHitter)
                jumpsRemaining -= 1;
        }
    }

    /*
    This method checks if a GameObject belongs in a layer mask.
    Since a layer mask in Unity is simply a binary string where each digit
    represents a layer (e.g. 000000001 means layer 0 is included in the mask),
    we can use bit shifting to check if the object's layer matches any of the layers
    in the layer mask.
    */
    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }

    public override void Die()
    {
        dashMeter.SetDefaultValue();
        cameraBounds.CameraCanMove = false;
        StopAllCoroutines();

        spriteRenderer.enabled = false;
        bc2d.enabled = false;
        shield.SetActive(false);
        sword.SetActive(false);

        GameObject deathParticles = Instantiate(deathEffect);
        deathParticles.transform.position = transform.position;

        ParticleSystem dpSystem = deathParticles.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule dpMain = dpSystem.main;

        dpMain.startColor = playerColor;

        Destroy(deathParticles, 2f);
        Invoke(nameof(InvokeRespawnEvent), 1f);
    }

    private void InvokeRespawnEvent() => GameManager.RespawnAll();

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

        //Clear inventory
        ironOwned = 0;
        batteryOwned = 0;
        swordOwned = 0;
        spikyBlocksOwned = 0;
        batteryBlockOwned = 0;

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
    public override void TakeDamage(float damage)
    {
        if (AbsorptionHealth > damage)
        {
            AbsorptionHealth -= damage;
            extraHealthBar.SetHealth(AbsorptionHealth);
        }
        else if (AbsorptionHealth > 0)
        {
            health -= damage - AbsorptionHealth;
            regularHealthBar.SetHealth(health);
            AbsorptionHealth = 0;
            extraHealthBar.SetHealth(AbsorptionHealth);
        }
        else
        {
            health -= damage;
            regularHealthBar.SetHealth(health);
        }

        VisualEffects.SetColor(gameObject, Color.red);
        if (isActiveAndEnabled) StartCoroutine(VisualEffects.FadeToColor(gameObject, 0.5f, Color.white));

        if (health <= 0)
            Die();
    }

    public override void Heal(int healAmount)
    {
        health += healAmount;

        //Health cannot overflow
        if (health > maxHealth)
            health = maxHealth;

        regularHealthBar.SetHealth(health);
    }

    //Sets the player's gravity scale while taking the gravity coefficient into account
    private void SetGravityScale(float newGravityScale)
    {
        rb2d.gravityScale = gravityCoefficient * newGravityScale;
    }

    //Switches the player's costume to the default costume
    public void SwitchToDefaultCostume()
    {
        spriteRenderer.sprite = playerSprite;
        ninjaAttachment.SetActive(false);
    }

    //Switches the player's costume to a ninja costume
    public void SwitchToNinjaCostume()
    {
        spriteRenderer.sprite = ninjaSprite;
        ninjaAttachment.SetActive(true);
    }

}