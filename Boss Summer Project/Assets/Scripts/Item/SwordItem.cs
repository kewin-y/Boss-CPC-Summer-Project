using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordItem : Item
{
    [SerializeField] private GameObject sword;

    private SwordController swordController;

    protected override void SummonEffect() {
        sword.SetActive(true);

        //Initialize the shield while passing this power up as an argument, so
        //the right shield health bar can be updated.
        swordController = sword.GetComponent<SwordController>();
        swordController.Initialize(this);
    }

    /*
    public override void RemoveEffect() {
        sword.SetActive(false);
    }
    */
}