using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI swordDisplay;
    [SerializeField] private TextMeshProUGUI spikyBlockDisplay;
    [SerializeField] private TextMeshProUGUI batteryBlockDisplay;

    private PlayerController playerScript;

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void UpdateText()
    {
        swordDisplay.SetText(playerScript.SwordOwned.ToString());
        spikyBlockDisplay.SetText(playerScript.SpikyBlocksOwned.ToString());
        batteryBlockDisplay.SetText(playerScript.BatteryBlockOwned.ToString());
    }
}
