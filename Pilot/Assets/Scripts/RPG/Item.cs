using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    protected string name;
    protected int cost;
    protected string description;
    public Mesh model {get; private set;}

    public Item(string name, string description, int cost, Mesh model)
    {
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.model = model;
    }
}
