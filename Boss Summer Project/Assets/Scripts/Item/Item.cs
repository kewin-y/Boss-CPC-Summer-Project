using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class from which all item scripts inherit.
//Every item must contain a ONE-TIME EFFECT (no lasting duration)
//and a reference to the player object.
public abstract class Item : MonoBehaviour
{
    protected GameObject player;
    
    protected PlayerController playerScript;

    protected void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        //Size of the power up = size of the player
        transform.localScale = player.transform.localScale;
    }
    
    //When the player collides with this item, hide it and add a one-time effect
    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            gameObject.SetActive(false);
            SummonEffect();
        }
    }

    //Abstract method that adds a one-time effect to the player
    protected abstract void SummonEffect();

    //When this item respawns (along with the player), reveal it again
    public void Respawn() {
        gameObject.SetActive(true);
    }
}
