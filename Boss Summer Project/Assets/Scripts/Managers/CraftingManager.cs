using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ironQuantityDisplay;
    // [SerializeField] private TextMeshProUGUI spikyBlockQuantityDisplay;
    private GameObject player;
    private PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ironQuantityDisplay.SetText(playerScript.IronOwned.ToString());
        // spikyBlockQuantityDisplay.SetText(playerScript.SpikyBlocksOwned.ToString());
    }
    
    public void CraftSpikyBlock() {
        if(playerScript.IronOwned >= 3) {
            playerScript.IronOwned -= 3;
            playerScript.SpikyBlocksOwned += 1;
            Debug.Log("Spiky Block Crafted!");
        }
        else Debug.Log("NOT ENOUGH IRON");
    }
    public void CraftSword() {
        if(playerScript.IronOwned >= 2) {
            playerScript.IronOwned -= 2;
            playerScript.SwordOwned += 1;
            Debug.Log("Sword Crafted!");
        }
        else 
            Debug.Log("NOT ENOUGH IRON");
        
    }
    public void CraftBatteryBlock() {

    }
}
