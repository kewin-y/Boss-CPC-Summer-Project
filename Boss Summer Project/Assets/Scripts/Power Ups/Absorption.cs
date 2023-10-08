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

    protected override void SummonEffect() {
        playerScript.AbsorptionHealth += absorptionAmount;

        //Health cannot overflow
        if (playerScript.AbsorptionHealth > playerScript.MaxAbsorptionHealth)
            playerScript.AbsorptionHealth = playerScript.MaxAbsorptionHealth;

        extraHealthBar.SetHealth(playerScript.AbsorptionHealth);
        StartCoroutine(RemoveOnNoHealth());
    }

    public override void RemoveEffect() {
        playerScript.AbsorptionHealth -= absorptionAmount;

        //Health cannot underflow
        if (playerScript.AbsorptionHealth < 0f)
            playerScript.AbsorptionHealth = 0f;

        extraHealthBar.SetHealth(playerScript.AbsorptionHealth);
    }

    private IEnumerator RemoveOnNoHealth()
    {
        yield return new WaitUntil(() => playerScript.AbsorptionHealth <= 0);
        RemoveEffectFully();
    }

}
