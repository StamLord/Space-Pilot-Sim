using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breather : MonoBehaviour
{
    [SerializeField] 
    private bool isBreathing;
    // [SerializeField] [Tooltip("Time in seconds before suffocation")]
    // private float noAirTime = 90;
    [SerializeField] 
    private float lastBreath;

    public delegate void breathChangeDelegate(bool state);
    public event breathChangeDelegate onBreathChange;

    public delegate void suffocateDelegate(float duration);
    public event suffocateDelegate onSuffocate;

    public void SetBreathing(bool state)
    {
        isBreathing = state;
        if(onBreathChange != null) 
            onBreathChange(isBreathing);
    }

    void Update()
    {
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
