using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//This power up gives the player some extra health in the second health bar
public class Absorption : PowerUp
{
    [SerializeField] private float absorptionAmount;
    [SerializeField] private HealthBar extraHealthBar;


    protected override void Start()
    {
        base.Start();
        extraHealthBar = GameObject.FindGameObjectWithTag("AbsorptionBar").GetComponent<HealthBar>();
    }

    private static int absorptionCollected;
    private static Absorption mostRecentlyCollectedPowerUp;
    private float remainingAbsorptionHealth; //amount of absorption health the player should have according to the absorptionCollected field (does not take into account any damage)

    void Update() {
        if (this != mostRecentlyCollectedPowerUp && mostRecentlyCollectedPowerUp != null) {
            RemoveEffect();
        }
    }
    protected override void SummonEffect() {
        mostRecentlyCollectedPowerUp = this;

        playerScript.AbsorptionHealth += absorptionAmount;

        //Health cannot overflow
        if (playerScript.AbsorptionHealth > playerScript.MaxAbsorptionHealth)
            playerScript.AbsorptionHealth = playerScript.MaxAbsorptionHealth;

        extraHealthBar.SetHealth(playerScript.AbsorptionHealth);
        StartCoroutine(RemoveOnNoHealth());

        absorptionCollected += 1;
    }

    public override void RemoveEffect() {
        absorptionCollected -= 1;

        remainingAbsorptionHealth = absorptionCollected * absorptionAmount;

        //If the player's absorption health is exceeding the maximum amount that they can have under
        //their current number of absorptionCollected, reduce their absorption health

        if (playerScript.AbsorptionHealth > remainingAbsorptionHealth) {

            playerScript.AbsorptionHealth = remainingAbsorptionHealth;

            //Health cannot underflow
            if (playerScript.AbsorptionHealth < 0f)
                playerScript.AbsorptionHealth = 0f;

            extraHealthBar.SetHealth(playerScript.AbsorptionHealth);
        }
    }

    private IEnumerator RemoveOnNoHealth()
    {
        yield return new WaitUntil(() => playerScript.AbsorptionHealth <= 0);
        RemoveEffectFully();
    }
}
