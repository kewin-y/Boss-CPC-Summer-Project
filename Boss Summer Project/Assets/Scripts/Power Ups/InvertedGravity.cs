using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedGravity : PowerUp
{
    protected override void SummonEffect() {

        //If player's gravity was already flipped, remove this effect now)
        if (playerScript.GravityCoefficient == -1)
            RemoveEffect();
        else {
            playerScript.GravityCoefficient = -1;
            player.transform.eulerAngles = new Vector3(0, 0, 180f);
            playerScript.flipHorizontal();
        }
    }

    protected override void RemoveEffect() {
        playerScript.GravityCoefficient = 1;
        player.transform.eulerAngles = Vector3.zero;
        playerScript.flipHorizontal();
    }
}
