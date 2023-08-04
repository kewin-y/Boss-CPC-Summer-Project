using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleJump : PowerUp
{
    protected override void SummonEffect() {
        playerScript.jumpsAvailable = playerScript.jumpsRemaining = 3;
    }

    public override void RemoveEffect() {
        playerScript.jumpsAvailable = playerScript.jumpsRemaining = 2;
    }
}
