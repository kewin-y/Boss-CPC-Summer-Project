using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject enemy;
    public GameObject projectile;

    public Transform player;

    public bool canCollide;
    public float moveSpeed;

    private Vector2 motion;

    private Rigidbody2D rb2d;

    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 motion = new Vector2(-1,0);

        if(gameObject.activeSelf){
            rb2d.AddForce(motion * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}