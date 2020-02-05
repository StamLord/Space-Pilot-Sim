using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatName 
{
    STRENGTH,
    DEXTIRITY,
    INTELLIGENCE,
    CHARISMA,
    LUCK
};

[System.Serializable]
public class Stat
{
    [SerializeField] private StatName _name;
    [SerializeField] private int _level;

    public StatName name { get{ return _name; }}
    public int level { get{ return _level; }}

    public Stat(StatName name)
    {
        _name = name;
        _level = 0;
    }

    public Stat(StatName name, int level)
    {
        _name = name;
        _level = level;
    }

    public void Increment(int amount)
    {
        _level += amount;
    }

}
