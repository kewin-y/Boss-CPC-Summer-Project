    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronResource : Item
{
    protected override void SummonEffect() {
        playerScript.IronOwned += 1;
        Debug.Log(playerScript.IronOwned);
    }
}
