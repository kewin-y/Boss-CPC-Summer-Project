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

    private float distanceTraveled;
    public float DistanceTraveled
    {
        get { return distanceTraveled; }
        set { distanceTraveled = value; }
    }

    public Statistics(int jumps, float distanceTraveled)
    {
        this.jumps = jumps;
        this.distanceTraveled = distanceTraveled;
    }
}