using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Statistics
{
    private int jumps;
    public int Jumps
    {
        get { return jumps; }
        set { jumps = value; }
    }

    private float distanceTravelled;
    public float DistanceTravelled
    {
        get { return distanceTravelled; }
        set { distanceTravelled = value; }
    }
    private int deaths;
    public int Deaths { get => deaths; set => deaths = value; }

    private int itemsCrafted;
    public int ItemsCrafted { get => itemsCrafted; set => itemsCrafted = value; }
    public Statistics(int jumps, float distanceTraveled, int deaths, int itemsCrafted)
    {
        this.jumps = jumps;
        this.distanceTravelled = distanceTraveled;
        this.deaths = deaths;
        this.itemsCrafted = itemsCrafted;
    }
}