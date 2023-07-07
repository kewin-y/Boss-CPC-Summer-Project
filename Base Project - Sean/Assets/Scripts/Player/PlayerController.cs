using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float coyoteTime; // Time between last grounded where the player can still jump midair; makes controller more fair and responsive
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector3 spawnPoint;

    private Rigidbody2D rb2d;
    private float xInput; // Variable for the x-input (a&d or left & right)
    private bool isGrounded; // If the player is on the ground 
    private float lastGrounded; // Or airtime; time since the player was last grounded
    private bool canJump;
    private bool wishJump; // Jump queueing; no holding down the button to jump repeatedly, but pressing before the player is grouded will make the square jump as soon as it lands
    private float playerSize = 0.45f; // Appears to be 0.5 but hitbox (box collider) is slightly smaller to make it more fair

    private bool canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lastGrounded = 0f;
        canJump = true;
        canDoubleJump = true;
        
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
            lastGrounded = 0f;

        getInput();

    }

    void getInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");   

        if(Input.GetKeyDown(jumpKey) && !wishJump) wishJump = true; // Player can queue a jump as long as the jump key (w) is held
        if(Input.GetKeyUp(jumpKey)) wishJump = false;

        if(wishJump && lastGrounded < coyoteTime && canJump) 
        {
            jump(); 
            wishJump = false;
            canJump = false; // canJump variable to prevent accidental double-jumping due to coyote time; implement a double-jumping mechanism that isn't actually a bug

            Invoke("resetJump", coyoteTime + 0.1f);
        } 
    }

    void resetJump() => canJump = true; // This is syntax for a one-liner method

    void jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
    }
    
    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(xInput * moveSpeed * 100f * Time.deltaTime, rb2d.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Kill")
        {
            transform.position = spawnPoint;
        }
    }

}
