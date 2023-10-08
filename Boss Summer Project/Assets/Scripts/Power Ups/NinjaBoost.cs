using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up makes the player undetectable by hazards (e.g. laser camera, enemies)
public class NinjaBoost : PowerUp
{
    private static int ninjaCollected;
    private static NinjaBoost mostRecentlyCollectedPowerUp;
    private const string PLAYER_LAYER_NAME = "Player";
    private const string NINJA_LAYER_NAME = "Ninja";

    void Update() {
        if (this != mostRecentlyCollectedPowerUp && mostRecentlyCollectedPowerUp != null) {
            RemoveEffect();
        }
    }
    protected override void SummonEffect() {
        mostRecentlyCollectedPowerUp = this;

        player.layer = LayerMask.NameToLayer(NINJA_LAYER_NAME);
        playerScript.SwitchToNinjaCostume();

        ninjaCollected += 1;
    }

    public override void RemoveEffect() {
        ninjaCollected -= 1;

        if (ninjaCollected == 0) {
            player.layer = LayerMask.NameToLayer(PLAYER_LAYER_NAME);
            playerScript.SwitchToDefaultCostume();
        }
    }
}
