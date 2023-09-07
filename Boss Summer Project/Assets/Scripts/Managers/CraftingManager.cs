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
        ironQuantityDisplay.SetText(playerScript.ironOwned.ToString());
        spikyBlockQuantityDisplay.SetText(playerScript.spikyBlocksOwned.ToString());
    }
    public void CraftSpikyBlock() {
        if(playerScript.ironOwned >= 3) {
            playerScript.ironOwned -= 3;
            playerScript.spikyBlocksOwned += 1;
        }
    }
    public void CraftSword() {
        if(playerScript.ironOwned >= 2) {
            playerScript.ironOwned -= 2;
            playerScript.swordOwned += 1;
        }
    }
    public void CraftBatteryBlock() {

    }
}
