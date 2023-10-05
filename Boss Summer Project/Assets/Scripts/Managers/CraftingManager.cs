using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ironQuantityDisplay;
    [SerializeField] private TextMeshProUGUI batteryQuantityDisplay;
    [SerializeField] private TextMeshProUGUI swordQuantityDisplay;
    [SerializeField] private TextMeshProUGUI spikyBlockQuantityDisplay;
    [SerializeField] private TextMeshProUGUI batteryBlockQuantityDisplay;
    private GameObject player;
    private PlayerController playerScript;
    private InventoryDisplay persistentDisplay;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        persistentDisplay = GameObject.FindGameObjectWithTag("InventoryDisplay").GetComponent<InventoryDisplay>();
    }

    // WHY IS THIS CALLED IN UPDATE
    // WHY WHY WHY WHY
    // Lol screw performance we gonna update tmpro assets every frame
    void Update()
    {
        ironQuantityDisplay.SetText(playerScript.IronOwned.ToString());
        batteryQuantityDisplay.SetText(playerScript.BatteryOwned.ToString());
        swordQuantityDisplay.SetText(playerScript.SwordOwned.ToString());
        spikyBlockQuantityDisplay.SetText(playerScript.SpikyBlocksOwned.ToString());
        batteryBlockQuantityDisplay.SetText(playerScript.BatteryBlockOwned.ToString());
        persistentDisplay.UpdateText();
    }

    public void CraftSpikyBlock()
    {
        if (playerScript.IronOwned >= 4)
        {
            playerScript.IronOwned -= 4;
            playerScript.SpikyBlocksOwned += 1;
            Debug.Log("Spiky Block Crafted!");
            StatisticsSystem.playerStats.ItemsCrafted++;
        }
        else Debug.Log("NOT ENOUGH IRON");
    }
    public void CraftSword()
    {
        if (playerScript.IronOwned >= 2)
        {
            playerScript.IronOwned -= 2;
            playerScript.SwordOwned += 1;
            Debug.Log("Sword Crafted!");
            StatisticsSystem.playerStats.ItemsCrafted++;
        }
        else
            Debug.Log("NOT ENOUGH IRON");

    }
    public void CraftBatteryBlock()
    {
        if (playerScript.IronOwned >= 4 && playerScript.BatteryOwned >= 1)
        {
            playerScript.IronOwned -= 4;
            playerScript.BatteryOwned -= 1;
            playerScript.BatteryBlockOwned += 1;
            Debug.Log("Battery Block Crafted!");
            StatisticsSystem.playerStats.ItemsCrafted++;
        }
        else
            Debug.Log("NOT ENOUGH INGREDIENTS");

    }

}
