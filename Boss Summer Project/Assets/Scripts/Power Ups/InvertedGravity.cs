using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedGravity : PowerUp
{
    protected override void SummonEffect() {

        //Toggle the player's IsFlipped flag. If the flag was previously true,
        //this will signal the previous inverted gravity power up to remove its effect.
        playerScript.IsFlipped = !playerScript.IsFlipped;

        playerScript.GravityCoefficient = -1;
        player.transform.eulerAngles = new Vector3(0, 0, 180f);
        playerScript.flipHorizontal();
        StartCoroutine(RemoveOnNextCollect());

    }

    //Waits until another inverted gravity power up is collected, then removes the effect
    private IEnumerator RemoveOnNextCollect() {
        yield return new WaitUntil(() => !playerScript.IsFlipped);
        RemoveEffectFully();
    }

    public override void RemoveEffect() {
        playerScript.GravityCoefficient = 1;
        player.transform.eulerAngles = Vector3.zero;
        playerScript.flipHorizontal();
    }
}
