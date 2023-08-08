using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorption : PowerUp
{
   //[SerializeField] private HealthBar healthBar;
    [SerializeField] private float absorptionAmount;
    [SerializeField] private HealthBar healthBar;

    protected override void SummonEffect() {
        playerScript.maxHealth += absorptionAmount;
        playerScript.health += absorptionAmount;
        healthBar.SetHealth(playerScript.health);
    }

    public override void RemoveEffect() {
        playerScript.maxHealth -= absorptionAmount;
        if(playerScript.health > 100){
            playerScript.health = 100;
        }
        healthBar.SetHealth(playerScript.health);
    }
}
