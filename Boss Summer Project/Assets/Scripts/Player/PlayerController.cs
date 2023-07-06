using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float coyoteTime;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector3 spawnPoint;

    private Rigidbody2D rb2d;
    private float xInput;
    private bool isGrounded;
    private float lastGrounded;
    private bool canJump;
    private bool wishJump;
    private float playerSize = 0.45f; // Appears to be 0.5 but hitbox (box collider) is slightly smaller to make it more fair

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lastGrounded = 0f;
        canJump = true;
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

        if(Input.GetKeyDown(jumpKey) && !wishJump) wishJump = true;
        if(Input.GetKeyUp(jumpKey)) wishJump = false;

        if(wishJump && lastGrounded < coyoteTime && canJump) 
        {
            jump(); 
            wishJump = false;
            canJump = false;

            Invoke("resetJump", coyoteTime + 0.1f);
        } 
    }

    void resetJump() => canJump = true;

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
