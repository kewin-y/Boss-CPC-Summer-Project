using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode dashKey = KeyCode.Mouse0;

    Vector2 up = new Vector2(0,1);
    Vector2 left = new Vector2(-1,0);
    Vector2 right = new Vector2(1,0);
    Vector2 down = new Vector2(0, -1);
    
    Quaternion neutralRotation = new Quaternion(0,0,0,0);

    public int jumpsRemaining;
    public float moveSpeed;
    public float dashForce;
    public float jumpHeight;
    public bool canDash;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        jumpsRemaining = 2;
        canDash = true;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = neutralRotation;

        if(Input.GetKeyDown(dashKey) && canDash){
            StartCoroutine(dash()); 
        }
        if(Input.GetKeyDown(jumpKey) && jumpsRemaining > 0){
            jumpsRemaining -= 1;
            rb2d.AddForce(up * jumpHeight, ForceMode2D.Impulse);
        } else if(Input.GetKey(leftKey)) {
            rb2d.AddForce(left * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        } else if(Input.GetKey(rightKey)){
            rb2d.AddForce(right * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    void OnCollisionEnter2D(Collision2D col){
        jumpsRemaining = 2;
    }
    IEnumerator dash(){
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 dirNormalized = dir.normalized;
        canDash = false;
        gameObject.tag = "Kill Block";
        rb2d.AddForce(dirNormalized * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        gameObject.tag = "Untagged";
        yield return new WaitForSeconds(4f);
        canDash = true;
    }
}