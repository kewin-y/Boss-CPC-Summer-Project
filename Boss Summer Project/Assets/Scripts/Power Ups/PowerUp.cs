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
    protected GameObject player; //The player object
    [SerializeField] private bool isInfinite;   //Whether the power up is permanent or not
    [SerializeField] private float duration;    //How long the boost lasts in seconds; disregarded if isInfinite
    [SerializeField] private GameObject powerUpBar;
    [SerializeField] private GridLayoutGroup powerUpBarGrid;    //The grid layout to which the power up "health bars" are added

    protected PlayerController playerScript;
    protected bool effectInProgress = false;
    
    private GameObject powerUpBarObj;
    private PowerUpBar powerUpBarScript;
    private float remainingDuration;

    public bool IsInfinite {
        get { return isInfinite; }
    }

    public float Duration {
        get { return duration; }
        set { duration = value; }
    }

    protected virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        powerUpBarGrid = GameObject.FindGameObjectWithTag("PowerUpGrid").GetComponent<GridLayoutGroup>();

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
        playerScript.PlayerEffects.Add(this);
        PowerUpBarManager.UpdatePowerUpIcons();

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
<<<<<<< Updated upstream
=======
        playerScript.PlayerEffects.Remove(this);
>>>>>>> Stashed changes
        RemoveEffect();
        PowerUpBarManager.UpdatePowerUpIcons();
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
<<<<<<< Updated upstream
    private void AddToUI() {
=======
    public void AddToUI() {
>>>>>>> Stashed changes
        powerUpBarObj = Instantiate(powerUpBar, powerUpBarGrid.transform);
        powerUpBarScript = powerUpBarObj.GetComponent<PowerUpBar>();

        powerUpBarScript.Setup(this);
    }

    //Removes the "health bar" for this power up from the UI
<<<<<<< Updated upstream
    public void RemoveFromUI() {
        Destroy(powerUpBarObj);
=======
    public void RemoveFromUI()
    {
        // Destroy every power up bar with the same power up ID
        // Only for the gravity power up since theres only 1 instance of a bar for every power up
        foreach(Transform t in powerUpBarGrid.transform)
        {
            if (string.Equals(t.name, powerUpId))
            {
                Destroy(t.gameObject);
            }
        }
        if (powerUpBarObj != null) {
            Destroy(powerUpBarObj);
        }
>>>>>>> Stashed changes
    }

    public void decreaseRemainingDuration(float time) {
        remainingDuration -= time;
    }
    public void UpdatePowerUpIcon() {
        PowerUpBarManager.addedPowerUps = new List<string>();

        RemoveFromUI();
        if (playerScript.PlayerEffects.Contains(this)) {
            AddToUI(); print(this.gameObject.name); print(PowerUpBarManager.addedPowerUps);
            PowerUpBarManager.addedPowerUps.Add(this.gameObject.name);
        }
    }
}