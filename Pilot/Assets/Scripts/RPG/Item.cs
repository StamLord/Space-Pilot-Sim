using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    protected string name;
    protected int cost;
    protected string description;

    public Item(string name, string description, int cost)
    {
        this.name = name;
        this.description = description;
        this.cost = cost;
    }
}
