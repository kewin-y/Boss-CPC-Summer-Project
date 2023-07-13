using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float coyoteTime; // Time between last grounded where the player can still jump midair; makes controller more fair and responsive
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float dashAmount;
    [SerializeField] private float dashCooldown;

    public Camera mainCam;

    private Rigidbody2D rb2d;
    private float xInput; // Variable for the x-input (a&d or left & right)
    private bool isGrounded; // If the player is on the ground 
    private float lastGrounded; // Or airtime; time since the player was last grounded
    private bool canJump;
    private bool wishJump; // Jump queueing; no holding down the button to jump repeatedly, but pressing before the player is grouded will make the square jump as soon as it lands
    private float playerSize = 0.45f; // Appears to be 0.5 but hitbox (box collider) is slightly smaller to make it more fair

    private bool doubleJump;
    private bool canDash;
    private bool isDashing;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lastGrounded = 0f;
        canJump = true;
        canDash = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize), 0f, Vector2.down, 0.1f, whatIsGround);

        if(!isGrounded)
        {
            lastGrounded += Time.deltaTime;
        }

        else
        {
            lastGrounded = 0f;
        }
        
        getInput();
    }

    void getInput()
    { 
        xInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(jumpKey) && !wishJump) wishJump = true; // Player can queue a jump as long as the jump key (w) is held
        if(Input.GetKeyUp(jumpKey)) wishJump = false;

        if(isGrounded && !Input.GetKey(jumpKey)) doubleJump = false;

        if(wishJump && canJump && (lastGrounded < coyoteTime || doubleJump)) 
        {
            jump(); 
            wishJump = false;
            canJump = false; // canJump variable to prevent accidental double-jumping due to coyote time; implement a double-jumping mechanism that isn't actually a bug
            doubleJump = !doubleJump;

            Invoke("resetJump", coyoteTime + 0.1f); // Resets the jump after the coyote time period
        }

        if(Input.GetKey(dashKey) && canDash)
        {
            StartCoroutine(dash());
        } 
    }

    void resetJump() => canJump = true; // This is syntax for a one-line method

    void jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
    }

    IEnumerator dash()
    {
        
        isDashing = true;
        canDash = false;

        rb2d.gravityScale = 0f;

        Vector2 distance = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 direction = distance.normalized;    

        rb2d.AddForce(direction * dashAmount, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);

        float afterDashVelo = direction.x / Mathf.Abs(direction.x) * moveSpeed;

        for(float t = 0.0f; t < 1f; t += Time.deltaTime / 0.1f)
        {
            rb2d.velocity = new Vector2(Mathf.Lerp(rb2d.velocity.x, afterDashVelo, t), Mathf.Lerp(rb2d.velocity.y, 0, t));
            yield return null;
        }

        isDashing = false;
        rb2d.gravityScale = 5f;
        doubleJump = true;

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Kill")
        {
            transform.position = spawnPoint; // tp the player to the spawnpoint
            
            Invoke("moveCamera", 0.5f);
        }
    }

    void moveCamera() => mainCam.transform.position = new Vector3(0, 0, -10f);

}
