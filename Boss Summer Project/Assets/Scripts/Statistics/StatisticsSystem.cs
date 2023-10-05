using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.IO;
using System;

public class StatisticsSystem
{
    public static Statistics playerStats;

    public static IDataService dataService = new JsonDataService();
    private const string PATH = "/player-stats.json";

    // Loads json file
    public static void LoadStatistics()
    {
        
        if(File.Exists(Application.persistentDataPath + PATH)) 
            playerStats = dataService.LoadData<Statistics>(PATH);
        
        else
        {
            playerStats = new(0, 0, 0, 0);
            SerializeJson();
        }
            
 
    }

    // Writes to json file
    public static void SerializeJson()
    {
        if (dataService.SaveData(PATH, playerStats))
        {
            Debug.Log("What a great success!");
        }

        else
        {
            Debug.LogError("Fail.");
        }
    }
}
