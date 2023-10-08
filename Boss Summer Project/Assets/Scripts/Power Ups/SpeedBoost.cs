using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up increases the player's speed
public class SpeedBoost : PowerUp
{
    private static int speedCollected;
    private static SpeedBoost mostRecentlyCollectedPowerUp;
    [SerializeField] private float boostMultiplier;
    private CameraBounds cameraBounds;

    protected override void Start()
    {
        base.Start();
        cameraBounds = Camera.main.GetComponent<CameraBounds>();
    }

    void Update() {
        if (this != mostRecentlyCollectedPowerUp && mostRecentlyCollectedPowerUp != null) {
            RemoveEffect();
        }
    }

    protected override void SummonEffect()
    {
        mostRecentlyCollectedPowerUp = this;

        playerScript.MoveSpeed = playerScript.MoveSpeed * boostMultiplier;
        cameraBounds.Zoom(-0.5f, 0.5f); // Makes the camera zoom out and it seems fast

        speedCollected += 1;
    }

    public override void RemoveEffect() {
        speedCollected -= 1;

        if (speedCollected == 0) {
            playerScript.MoveSpeed = playerScript.MoveSpeed / boostMultiplier;
            cameraBounds.Zoom(0.5f, 0.5f); // Lerp this camera thing somehow
        }
    }
}
