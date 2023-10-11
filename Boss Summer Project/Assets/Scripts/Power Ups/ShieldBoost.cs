using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This power up gives the player a shield which blocks incoming projectiles
public class ShieldBoost : PowerUp
{
    [SerializeField] private GameObject shield;

    private ShieldController shieldController;

    protected override void Start()
    {
        base.Start();
        // Lol this sucks
        shield = GameObject.FindGameObjectWithTag("ShieldPivot").transform.GetChild(0).gameObject;
    }

    protected override void SummonEffect() {
        shield.SetActive(true);

        //Initialize the shield while passing this power up as an argument, so
        //the right shield health bar can be updated.
        shieldController = shield.GetComponent<ShieldController>();
        shieldController.Initialize(this);
    }

    public override void RemoveEffect() {
        shield.SetActive(false);
    }
}
