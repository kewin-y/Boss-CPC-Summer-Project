using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]

//Abstract class from which all power up scripts inherit.
//Every power up must have a duration (if not permanent), a reference to the player object,
//and a reference to the power up "health bar"
public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] protected GameObject player; //The player object
    [SerializeField] private bool isInfinite;   //Whether the power up is permanent or not
    [SerializeField] private float duration;    //How long the boost lasts in seconds; disregarded if isInfinite
    [SerializeField] private GameObject powerUpBar;
    [SerializeField] private GridLayoutGroup powerUpBarGrid;    //The grid layout to which the power up "health bars" are added

    protected PlayerController playerScript;
    protected bool effectInProgress = false;
    
    private GameObject powerUpBarObj;
    private PowerUpBar powerUpBarScript;

    public bool IsInfinite {
        get { return isInfinite; }
    }

    public float Duration {
        get { return duration; }
        set { duration = value; }
    }

    protected void Start() {
        playerScript = player.GetComponent<PlayerController>();

        //Size of the power up = size of the player
        transform.localScale = player.transform.localScale;
    }
    
    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Collect();
            StartCoroutine(EffectSequence());
        }
    }

    protected void Collect() {
        //Make this power up disappear and no longer be collectable
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //TODO: particle effects
    }

    //Runs when the power up is collected. Summons the effect, then removes it after the duration
    //has passed IF the effect is not infinite/permanent.
    protected IEnumerator EffectSequence() {
        SummonEffect();
        AddToUI();

        effectInProgress = true;

        if (!isInfinite) {
            yield return StartCoroutine(WaitUntilWearsOff(duration));
            RemoveEffectFully();
        }
    }

    //For timed power ups: this method waits the appropriate duration while
    //updating the state of the power up bar every 0.1 seconds.
    protected IEnumerator WaitUntilWearsOff(float maxDuration) {
        float timeElapsed = 0;

        while (timeElapsed < maxDuration) {
            powerUpBarScript.SetDurationLeft(maxDuration - timeElapsed);

            //Update the power up bar every tenth of a second (0.1s)
            yield return new WaitForSeconds(0.1f);
            timeElapsed += 0.1f;
        }
    }

    //Method that allows direct changes to the state of the power up bar
    public void SetDurationLeft(float durationLeft) {
        powerUpBarScript.SetDurationLeft(durationLeft);
    }

    /*
    Calls the abstract RemoveEffect() method plus common features:
    - Remove the power up bar from the UI
    - Disable the effectInProgress flag
    - Set active to false
    */
    public void RemoveEffectFully() {
        RemoveEffect();
        RemoveFromUI();
        effectInProgress = false;
        gameObject.SetActive(false);
    }

    //Abstract method that adds the effect to the player
    protected abstract void SummonEffect();

    //Abstract method that removes the effect from the player
    public abstract void RemoveEffect();

    public void Respawn() {
        StopAllCoroutines();
        
        if (effectInProgress)
            RemoveEffectFully();

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
    public void RemoveFromUI() {
        Destroy(powerUpBarObj);
    }
}
