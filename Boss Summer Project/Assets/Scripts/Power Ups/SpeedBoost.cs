using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField] private float boostMultiplier;
    [SerializeField] private CameraBounds cameraBounds;

    protected override void SummonEffect() {
        playerScript.MoveSpeed = playerScript.MoveSpeed * boostMultiplier;
        cameraBounds.Zoom(-0.5f, 0.5f); // Makes the camera zoom out and it seems fast
    }

    public override void RemoveEffect() {
        playerScript.MoveSpeed = playerScript.MoveSpeed / boostMultiplier;
        cameraBounds.Zoom(0.5f, 0.5f); // Lerp this camera thing somehow
    }
}
