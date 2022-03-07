using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breather : MonoBehaviour
{
    [SerializeField] 
    private bool isBreathing;
    
    [SerializeField] 
    private float lastBreath;

    [SerializeField] 
    private List<OxygenArea> oxygenAreas;

    public delegate void breathChangeDelegate(bool state);
    public event breathChangeDelegate onBreathChange;

    public delegate void suffocateDelegate(float duration);
    public event suffocateDelegate onSuffocate;

    public void EnterOxygenArea(OxygenArea area)
    {
        oxygenAreas.Add(area);
    }

    public void ExitOxygenArea(OxygenArea area)
    {
        if(oxygenAreas.Contains(area))
            oxygenAreas.Remove(area);
    }

    private void SetBreathing(bool state)
    {
        isBreathing = state;
        if(onBreathChange != null) 
            onBreathChange(isBreathing);
    }

    private void UpdateBreathing()
    {
        if(oxygenAreas.Count == 0)
        {   
            SetBreathing(false);
            return;
        }

        foreach(OxygenArea o in oxygenAreas)
        {
            if(o.Oxygen)
            {
                SetBreathing(true);
                return;
            }
        }
        
        SetBreathing(false);
    }

    void Update()
    {
        UpdateBreathing();
        Suffocate();
    }

    void Suffocate()
    {
        if(isBreathing)
        {
            lastBreath = Time.time;
            return;
        }

        if(onSuffocate != null)        
            onSuffocate(Time.time - lastBreath);
    }
}
