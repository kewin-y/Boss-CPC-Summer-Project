using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up makes the player undetectable by hazards (e.g. laser camera, enemies)
public class NinjaBoost : PowerUp
{
    private const string PLAYER_LAYER_NAME = "Player";
    private const string NINJA_LAYER_NAME = "Ninja";

    protected override void SummonEffect() {
        player.layer = LayerMask.NameToLayer(NINJA_LAYER_NAME);
        playerScript.SwitchToNinjaCostume();
    }

    public override void RemoveEffect() {
        player.layer = LayerMask.NameToLayer(PLAYER_LAYER_NAME);
        playerScript.SwitchToDefaultCostume();
    }
}
