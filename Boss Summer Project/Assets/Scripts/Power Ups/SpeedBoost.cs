using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up increases the player's speed
public class SpeedBoost : PowerUp
{
    [SerializeField] private float boostMultiplier;
    private CameraBounds cameraBounds;
    private float fast;
    private float defaultSpeed;

    protected override void Start()
    {
        base.Start();
        cameraBounds = Camera.main.GetComponent<CameraBounds>();
        fast = playerScript.MoveSpeed * boostMultiplier;
        defaultSpeed = playerScript.MoveSpeed;
    }


    protected override void SummonEffect()
    {
        // mostRecentlyCollectedPowerUp = this;
        playerScript.MoveSpeed = fast;
        cameraBounds.ZoomOut(); // Makes the camera zoom out and it seems fast
        cameraBounds.BufferValue = 1.25f;
        print(cameraBounds.BufferValue);

    }

    public override void RemoveEffect()
    {
        playerScript.MoveSpeed = defaultSpeed;
        cameraBounds.ZoomIn();
        cameraBounds.BufferValue = cameraBounds.DefaultBufferValue;
        print(cameraBounds.BufferValue);
    }

    public override void RemoveNoVisual()
    {
        playerScript.MoveSpeed = defaultSpeed;
        cameraBounds.BufferValue = cameraBounds.DefaultBufferValue;
        print(cameraBounds.BufferValue);
    }
}
