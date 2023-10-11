using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up allows the player to jump thrice
public class TripleJump : PowerUp
{
    private static int tripleJumpCollected;


    protected override void SummonEffect()
    {
        playerScript.JumpsAvailable = playerScript.JumpsRemaining = 3;

        tripleJumpCollected += 1;
    }

    public override void RemoveEffect()
    {
        tripleJumpCollected -= 1;

        if (tripleJumpCollected == 0)
        {
            playerScript.JumpsAvailable = playerScript.JumpsRemaining = 2;
        }
    }

    public override void RemoveNoVisual()
    {
        RemoveEffect();
    }
}
