using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up increases the player's speed
public class SpeedBoost : PowerUp
{
    [SerializeField] private float boostMultiplier;
    private CameraBounds cameraBounds;

    protected override void Start()
    {
        base.Start();
        cameraBounds = Camera.main.GetComponent<CameraBounds>();
    }

    protected override void SummonEffect()
    {
        playerScript.MoveSpeed = playerScript.MoveSpeed * boostMultiplier;
        cameraBounds.Zoom(-0.5f, 0.5f); // Makes the camera zoom out and it seems fast
    }

    public override void RemoveEffect()
    {
        playerScript.MoveSpeed = playerScript.MoveSpeed / boostMultiplier;
        cameraBounds.Zoom(0.5f, 0.5f); // Lerp this camera thing somehow
    }
}
