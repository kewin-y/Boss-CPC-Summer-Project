using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorption : PowerUp
{
    [SerializeField] private float absorptionAmount;

    protected override void SummonEffect() {
        playerScript.maxHealth += absorptionAmount;
    }

    public override void RemoveEffect() {
        playerScript.maxHealth -= absorptionAmount;
    }
}
