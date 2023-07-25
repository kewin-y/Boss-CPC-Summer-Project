using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField] private float boostMultiplier;

    protected override void SummonEffect() {
        playerScript.MoveSpeed = playerScript.MoveSpeed * boostMultiplier;
        Camera.main.orthographicSize = 6.5f; // Makes the camera zoom out and it seems fast
    }

    protected override void RemoveEffect() {
        playerScript.MoveSpeed = playerScript.MoveSpeed / boostMultiplier;
        Camera.main.orthographicSize = 6f; // Lerp this camera thing somehow
    }
}
