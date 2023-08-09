using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorption : PowerUp
{
    [SerializeField] private float absorptionAmount;
    [SerializeField] private HealthBar extraHealthBar;

    protected override void SummonEffect() {
        playerScript.absorptionHealth += absorptionAmount;
        extraHealthBar.SetHealth(absorptionAmount);
        StartCoroutine(RemoveOnNoHealth());
    }

    public override void RemoveEffect() {
        playerScript.absorptionHealth = 0f;
        extraHealthBar.SetHealth(0.0f);
    }

    private IEnumerator RemoveOnNoHealth()
    {
        yield return new WaitUntil(() => playerScript.absorptionHealth <= 0);
        RemoveEffectFully();
    }

}
