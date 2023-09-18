using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ironQuantityDisplay;
    [SerializeField] private TextMeshProUGUI spikyBlockQuantityDisplay;
    [SerializeField] private GameObject player;
    private PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ironQuantityDisplay.SetText(playerScript.IronOwned.ToString());
        spikyBlockQuantityDisplay.SetText(playerScript.SpikyBlocksOwned.ToString());
    }
    public void CraftSpikyBlock() {
        if(playerScript.IronOwned >= 3) {
            playerScript.IronOwned -= 3;
            playerScript.SpikyBlocksOwned += 1;
        }
    }
    public void CraftSword() {
        if(playerScript.IronOwned >= 2) {
            playerScript.IronOwned -= 2;
            playerScript.SwordOwned += 1;
        }
    }
    public void CraftBatteryBlock() {

    //}
}
