using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleJump : PowerUp
{
    protected override void SummonEffect() {
        playerScript.jumpsAvailable = playerScript.jumpsRemaining = 3;
    }

    protected override void RemoveEffect() {
        playerScript.jumpsAvailable = playerScript.jumpsRemaining = 2;
    }
}
