using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpBarManager : MonoBehaviour
{
    protected GameObject player; //The player object
    protected static PlayerController playerScript;
    public static List<string> addedPowerUps;
    private static UnityEvent updatePowerUpIconsEvent;
    private Transform powerUps;

    //Add all power ups as listeners for the update event
    //NOTE: This makes it unnecessary to do so in the inspector.
    private void SetupUpdateEvent()
    {
        updatePowerUpIconsEvent = new UnityEvent();

        //Add power ups as listeners
        for (int i = 0; i < powerUps.childCount; i++)
        {
            PowerUp powerUpScript = powerUps.GetChild(i).GetComponent<PowerUp>();
            updatePowerUpIconsEvent.AddListener(powerUpScript.UpdatePowerUpIcon);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        powerUps = GameObject.Find("Power Ups").transform;
        
        SetupUpdateEvent();
    }

    public static void UpdatePowerUpIcons()
    {
        updatePowerUpIconsEvent.Invoke();
    }
}
