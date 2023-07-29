using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoost : PowerUp
{
    [SerializeField] private GameObject shield;

    protected override void SummonEffect() {
        shield.SetActive(true);
    }

    protected override void RemoveEffect() {
        shield.SetActive(false);
    }
}
