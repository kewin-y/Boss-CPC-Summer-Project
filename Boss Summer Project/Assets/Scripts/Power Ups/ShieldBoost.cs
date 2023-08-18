using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up gives the player a shield which blocks incoming projectiles
public class ShieldBoost : PowerUp
{
    [SerializeField] private GameObject shield;

    protected override void SummonEffect() {
        shield.SetActive(true);
    }

    public override void RemoveEffect() {
        shield.SetActive(false);
    }
}
