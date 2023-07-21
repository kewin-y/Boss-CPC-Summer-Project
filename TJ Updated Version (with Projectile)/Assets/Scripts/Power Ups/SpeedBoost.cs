using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField] private float boostMultiplier;

    protected override void SummonEffect() {
        playerScript.MoveSpeed = playerScript.MoveSpeed * boostMultiplier;
    }

    protected override void RemoveEffect() {
        playerScript.MoveSpeed = playerScript.MoveSpeed / boostMultiplier;
    }
}
