using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTJ : MonoBehaviour
{
    public GameObject particles;
    public Enemy enemy;
    public GameObject projectile;

    [SerializeField] private Vector3 spawnPoint;

    private bool canCollide = true;
    public bool CanCollide{
        get{return canCollide;}
        set{canCollide = value;}
    }

    [SerializeField] private KeyCode jumpKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;
    //[SerializeField] private KeyCode downKey = KeyCode.DownArrow;
    [SerializeField] private KeyCode dashKey = KeyCode.Mouse0;
    
    Quaternion neutralRotation = new Quaternion(0,0,0,0);

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canDash;
    [SerializeField] private bool isJumping;
    [SerializeField] private int jumpsRemaining;
    [SerializeField] private float lastGrounded;

//Inspector input
    [SerializeField] private float coyoteTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float shieldDuration;
    [SerializeField] private float boostDuration;
    [SerializeField] private float tripleJumpDuration;

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private bool shieldActive;
    private int jumpsAvailable;
    private float boostFactor;
    private float playerSize = 1f;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        canCollide = true;
        boostFactor = 1;
        jumpsRemaining = jumpsAvailable = 2;
        canDash = true;
        shieldActive = false;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = neutralRotation;

        isGrounded = Physics2D.BoxCast(transform.position, new Vector2(playerSize-0.1f, playerSize), 0f, Vector2.down, 0.01f, whatIsGround);
        
        if(!isGrounded){
            lastGrounded += Time.deltaTime;
        } else {
            lastGrounded = 0f;
        }
        getInput();
    }

    void getInput()
    {
        if(Input.GetKeyDown(dashKey) && canDash){
            StartCoroutine(dash()); 
        }
        if(Input.GetKeyDown(jumpKey) && jumpsRemaining > 0){
            jumpsRemaining -= 1;
            if (lastGrounded < coyoteTime || isJumping){
                isJumping = true;
                rb2d.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            }
        }
        //horizontal movement
        if(Input.GetKey(leftKey)) {
            rb2d.AddForce(Vector2.left * moveSpeed * boostFactor * Time.deltaTime, ForceMode2D.Impulse);
        } else if(Input.GetKey(rightKey)){
            rb2d.AddForce(Vector2.right * moveSpeed * boostFactor * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    //This method will run when the player collides with something
    void OnCollisionEnter2D(Collision2D col) {
        jumpsRemaining = jumpsAvailable;
        isJumping = false;

        if(col.gameObject.tag == "Powerup"){
            if(col.gameObject.name == "Speed Boost"){
                boostFactor = 5;
                Invoke("deactivateBoost", boostDuration);
            } else if(col.gameObject.name == "Triple Jump"){
                jumpsRemaining = jumpsAvailable = 3;
                Invoke("deactivateTripleJump", tripleJumpDuration);
            }
            if(col.gameObject.name == "Shield"){
                shieldActive = true;
                Invoke("deactivateShield", shieldDuration);
            }
            col.gameObject.SetActive(false);
        } else if(col.gameObject.layer == 9 && shieldActive){
            return;
        } else if(col.gameObject.tag == "Kill" && canCollide){
            GameObject deathParticles = Instantiate(particles) as GameObject;
            deathParticles.transform.position = col.GetContact(0).point;

            //Set the colour of the particles to the player's colour
            ParticleSystem dpSystem = deathParticles.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule dpMain = dpSystem.main;
            dpMain.startColor = gameObject.GetComponent<SpriteRenderer>().color;
            
            gameObject.SetActive(false);

            //Destroy the particles after 2 seconds
            Destroy(deathParticles, 2);

            //Respawn the Player
            Invoke("respawn", 2.0f);
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
    void deactivateBoost(){
        boostFactor = 1;
    }
    void deactivateTripleJump(){
        if(isJumping){
            jumpsAvailable = 2;
        } else {
            jumpsRemaining = jumpsAvailable = 2;
        }
    }
    void deactivateShield(){
        shieldActive = false;
    }
    void respawn(){
        gameObject.SetActive(true);
        transform.position = spawnPoint;
    }
}