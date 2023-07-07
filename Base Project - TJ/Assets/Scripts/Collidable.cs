using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public GameObject particles;
    public Enemy enemy;
    public GameObject projectile;

    private bool canCollide = true;
    public bool CanCollide{
        get{return canCollide;}
        set{canCollide = value;}
    }

    void Start()
    {
        canCollide = true;
    }

    //This method will run when the player collides with something
    void OnCollisionEnter2D(Collision2D col) {
        if(gameObject.tag == "Enemy" && col.gameObject.layer == 6){
            return;
        }
        else if(col.gameObject.tag == "Kill Block" && canCollide){
            GameObject deathParticles = Instantiate(particles) as GameObject;
            deathParticles.transform.position = col.GetContact(0).point;

            //Set the colour of the particles to the player's colour
            ParticleSystem dpSystem = deathParticles.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule dpMain = dpSystem.main;
            dpMain.startColor = gameObject.GetComponent<SpriteRenderer>().color;

            gameObject.SetActive(false);

            //Destroy the particles after 2 seconds
            Destroy(deathParticles, 2);
        }
    }
}
