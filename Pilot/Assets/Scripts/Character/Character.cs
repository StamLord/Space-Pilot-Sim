using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private string charName;
    
    [Space(10)]

    [SerializeField] private int exp;
    [SerializeField] private int nextLevel = 100;
    [SerializeField] private int level;

    [Space(10)]

    [SerializeField] private int statPointsPerLevel = 2;
    [SerializeField] private int statPoints;
    [SerializeField] private Stat[] stats = {
        new Stat(StatName.STRENGTH),
        new Stat(StatName.DEXTIRITY),
        new Stat(StatName.INTELLIGENCE),
        new Stat(StatName.CHARISMA),
        new Stat(StatName.LUCK)
    };

    public void AddExperience(int amount)
    {
        exp += amount;
        if(exp >= nextLevel)
            LevelUp();
    }

    private void LevelUp()
    {
        if(exp < nextLevel) return;

        exp -= nextLevel;
        level++;
        statPoints += statPointsPerLevel;
    }

    private Stat FindStat(StatName statName)
    {
        foreach(Stat s in stats)
        {
            if(s.name == statName)
                return s;
        }

        return null;
    }

    public int GetStat(StatName statName)
    {
        Stat stat = FindStat(statName);

        if(stat != null) 
            return stat.level;
        else 
            return -1;
    }

    public bool IncrementStat(StatName statName, int amount)
    {
        Stat stat = FindStat(statName);
        if(stat != null && amount <= statPoints)
        {
            statPoints -= amount;
            stat.Increment(amount);
            return true;
        }
        else
            return false;
    }
}
