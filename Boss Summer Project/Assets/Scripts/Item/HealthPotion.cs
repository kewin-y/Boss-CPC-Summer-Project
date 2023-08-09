using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Item class will recover a small amount of the player's health
public class HealthPotion : Item
{
    [SerializeField] private int healAmount;

    protected override void SummonEffect() {
        playerScript.Heal(healAmount);
    }
}
