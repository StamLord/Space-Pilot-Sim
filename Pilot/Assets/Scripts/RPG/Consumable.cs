using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public float hungerValue {get; private set;}
    public float waterValue {get; private set;}
    public int healthValue {get; private set;}

    public Consumable(string name, string description, int cost, float hungerValue, float waterValue, int healthValue) : base(name, description, cost)
    {
        this.hungerValue = healthValue;
        this.waterValue = waterValue;
        this.healthValue = healthValue;
    }

}
