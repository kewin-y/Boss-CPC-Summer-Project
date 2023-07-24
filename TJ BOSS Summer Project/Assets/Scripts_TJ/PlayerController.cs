using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode jumpKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;
    //[SerializeField] private KeyCode downKey = KeyCode.DownArrow;
    [SerializeField] private KeyCode dashKey = KeyCode.Mouse0;
    
    Quaternion neutralRotation = new Quaternion(0,0,0,0);

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canDash;
    [SerializeField] private int jumpsRemaining;
    [SerializeField] private float lastGrounded;

//Inspector input
    [SerializeField] private float coyoteTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float boostDuration;
    [SerializeField] private float tripleJumpDuration;

    [SerializeField] private LayerMask whatIsGround;

    private int jumpsAvailable;
    private float boostFactor;
    private float playerSize = 0.45f;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        boostFactor = 1;
        jumpsRemaining = jumpsAvailable = 2;
        canDash = true;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = neutralRotation;

        isGrounded = Physics2D.BoxCast(transform.position, new Vector2(playerSize - 0.1f, playerSize), 0f, Vector2.down, 1f, whatIsGround);
        
        if(!isGrounded){
            lastGrounded += Time.deltaTime;
        } else {
            lastGrounded = 0f;
            jumpsRemaining = jumpsAvailable;
        }
        getInput();
    }

    void getInput()
    {
        if(Input.GetKeyDown(dashKey) && canDash){
            StartCoroutine(dash()); 
        }
        if(Input.GetKeyDown(jumpKey) && jumpsRemaining > 0 && lastGrounded < coyoteTime){
            jumpsRemaining -= 1;
            rb2d.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);

        //horizontal movement
        } else if(Input.GetKey(leftKey)) {
            rb2d.AddForce(Vector2.left * moveSpeed * boostFactor * Time.deltaTime, ForceMode2D.Impulse);
        } else if(Input.GetKey(rightKey)){
            rb2d.AddForce(Vector2.right * moveSpeed * boostFactor * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    IEnumerator dash(){
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 dirNormalized = dir.normalized;

        canDash = false;
        gameObject.tag = "Kill";
        rb2d.AddForce(dirNormalized * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        gameObject.tag = "Untagged";
        yield return new WaitForSeconds(4f);
        canDash = true;
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Powerup"){
            if(col.gameObject.name == "Boost"){
                boostFactor = 5;
                Invoke("deactivateBoost", boostDuration);
            } else if(col.gameObject.name == "Triple Jump"){
                jumpsAvailable = 3;
                Invoke("deactivateTripleJump", tripleJumpDuration);
            }
        }
    }
    void deactivateBoost(){
        boostFactor = 1;
    }
    void deactivateTripleJump(){
        jumpsAvailable = 2;
    }
}