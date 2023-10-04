using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.IO;

public class StatisticsSystem
{
    Statistics playerStats;
    public Statistics PlayerStats { get => playerStats; set => playerStats = value; }

    private IDataService dataService = new JsonDataService();
    private const string PATH = "/player-stats.json";

    public StatisticsSystem()
    {
        LoadStatistics();
    }

    // Loads json file
    public void LoadStatistics()
    {
        playerStats = dataService.LoadData<Statistics>(PATH);
    }

    // Writes to json file
    public void SerializeJson()
    {
        if(dataService.SaveData(PATH, playerStats))
        {
            Debug.Log("What a great success!");
        }

        else
        {
            Debug.LogError("Fail.");
        }
    }
}
