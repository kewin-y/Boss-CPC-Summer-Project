using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up allows the player to jump thrice
public class TripleJump : PowerUp
{
    protected override void SummonEffect() {
        playerScript.JumpsAvailable = playerScript.JumpsRemaining = 3;
    }

    public override void RemoveEffect() {
        playerScript.JumpsAvailable = playerScript.JumpsRemaining = 2;
    }
}
