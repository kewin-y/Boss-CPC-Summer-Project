using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up allows the player to jump thrice
public class TripleJump : PowerUp
{
    private static int tripleJumpCollected;
    private static TripleJump mostRecentlyCollectedPowerUp;

    void Update() {
        if (this != mostRecentlyCollectedPowerUp && mostRecentlyCollectedPowerUp != null) {
            RemoveEffect();
        }
    }

    protected override void SummonEffect() {
        mostRecentlyCollectedPowerUp = this;

        playerScript.JumpsAvailable = playerScript.JumpsRemaining = 3;
        
        tripleJumpCollected += 1;
    }

    public override void RemoveEffect() {
        tripleJumpCollected -= 1;

        if (tripleJumpCollected == 0) {
            playerScript.JumpsAvailable = playerScript.JumpsRemaining = 2;
        }
    }
}
