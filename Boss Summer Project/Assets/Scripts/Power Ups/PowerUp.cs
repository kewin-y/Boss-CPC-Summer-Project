using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected GameObject player; //The player object
    [SerializeField] private bool isInfinite;
    [SerializeField] private float duration;    //How long the boost lasts in seconds; disregarded if isInfinite

    protected PlayerController playerScript;
    protected bool effectInProgress = false;

    protected void Start() {
        playerScript = player.GetComponent<PlayerController>();

        //Size of the power up = size of the player
        transform.localScale = player.transform.localScale;
    }
    
    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Collect();
            StartCoroutine(AddEffect());
        }
    }

    protected void Collect() {
        //Make this power up disappear and no longer be collectable
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //TODO: particle effects
    }

    protected IEnumerator AddEffect() {
        SummonEffect();
        effectInProgress = true;

        if (!isInfinite) {
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
            RemoveEffect();
            effectInProgress = false;
        }
    }

    protected abstract void SummonEffect();
    protected abstract void RemoveEffect();

    public void Respawn() {
        StopAllCoroutines();
        
        if (effectInProgress) {
            RemoveEffect();
            effectInProgress = false;
        }

        gameObject.SetActive(true);

        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
