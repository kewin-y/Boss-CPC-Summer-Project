using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField] private float boostMultiplier;

    protected override void SummonEffect() {
        playerScript.BaseSpeed = playerScript.BaseSpeed * boostMultiplier;
    }

    protected override void RemoveEffect() {
        playerScript.BaseSpeed = playerScript.BaseSpeed / boostMultiplier;
    }
}
