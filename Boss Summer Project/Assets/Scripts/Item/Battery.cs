using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Item
{
    protected override void SummonEffect() {
        playerScript.BatteryOwned += 1;
    }
}
