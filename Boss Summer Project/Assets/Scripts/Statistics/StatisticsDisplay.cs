using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsDisplay : MonoBehaviour
{
    TextMeshProUGUI text;
    void Awake()
    {
        StatisticsSystem.LoadStatistics();
    }

    void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.SetText($"Jumps: {StatisticsSystem.playerStats.Jumps}" 
        + $"<br>Distance Travelled: {StatisticsSystem.playerStats.DistanceTravelled}m"
        + $"<br>Deaths: {StatisticsSystem.playerStats.Deaths}"
        + $"<br>Items Crafted: {StatisticsSystem.playerStats.ItemsCrafted}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
