using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private float remainingDuration;
    public float RemainingDuration
    {
        get { return remainingDuration; }
        set { remainingDuration = value; }
    }

    public Effect(string name, int remainingDuration) 
    {
        this.name = name;
        this.remainingDuration = remainingDuration;
    }

    public void decreaseRemainingDuration(float time) {
        remainingDuration -= time;
    }
}