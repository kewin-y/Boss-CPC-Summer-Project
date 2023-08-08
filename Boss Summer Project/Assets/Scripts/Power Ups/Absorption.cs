using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorption : PowerUp
{
    [SerializeField] private float absorptionAmount;
    [SerializeField] private HealthBar extraHealthBar;

    protected override void SummonEffect() {
        playerScript.absorptionHealth = absorptionAmount;
        extraHealthBar.SetHealth(absorptionAmount);
    }

    public override void RemoveEffect() {
        playerScript.absorptionHealth = 0.0f;
        extraHealthBar.SetHealth(0.0f);
    }
}
