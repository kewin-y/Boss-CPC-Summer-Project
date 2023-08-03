using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected GameObject player; //The player object
    [SerializeField] private bool isInfinite;
    [SerializeField] private float duration;    //How long the boost lasts in seconds; disregarded if isInfinite
    [SerializeField] private GameObject powerUpBar;
    [SerializeField] private GridLayoutGroup powerUpBarGrid;    //The grid layout to which the power up "health bars" are added

    public bool IsInfinite {
        get { return isInfinite; }
    }

    public float Duration {
        get { return duration; }
        set { duration = value; }
    }

    protected PlayerController playerScript;
    protected bool effectInProgress = false;
    private GameObject powerUpBarObj;
    private PowerUpBar powerUpBarScript;


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
        AddToUI();
        effectInProgress = true;

        if (!isInfinite) {
            yield return StartCoroutine(WaitUntilWearsOff(duration));
            gameObject.SetActive(false);
            RemoveFromUI();
            RemoveEffect();
            effectInProgress = false;
        }
    }

    protected IEnumerator WaitUntilWearsOff(float maxDuration) {
        float timeElapsed = 0;

        while (timeElapsed < maxDuration) {
            powerUpBarScript.SetDurationLeft(maxDuration - timeElapsed);

            //Update the power up bar every tenth of a second (0.1s)
            yield return new WaitForSeconds(0.1f);
            timeElapsed += 0.1f;
        }
    }

    protected abstract void SummonEffect();
    protected abstract void RemoveEffect();

    public void Respawn() {
        StopAllCoroutines();
        
        if (effectInProgress) {
            RemoveFromUI();
            RemoveEffect();
            effectInProgress = false;
        }

        gameObject.SetActive(true);

        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    //Adds the "health bar" for this power up to the UI
    private void AddToUI() {
        powerUpBarObj = Instantiate(powerUpBar, powerUpBarGrid.transform);
        powerUpBarScript = powerUpBarObj.GetComponent<PowerUpBar>();

        powerUpBarScript.Setup(this);
    }

    //Removes the "health bar" for this power up from the UI
    private void RemoveFromUI() {
        Destroy(powerUpBarObj);
    }
}
