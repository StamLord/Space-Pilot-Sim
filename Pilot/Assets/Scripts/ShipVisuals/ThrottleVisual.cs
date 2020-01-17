using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleVisual : MonoBehaviour
{   
    public Transform throttle;
    public Vector3 rotationMin;
    public Vector3 rotationMax;
    
    public Engine engine;

    void Awake()
    {
        if(engine == null)
            engine = GetComponent<Engine>();
        
        engine.OnThrottleChange += UpdateRotation;

        UpdateRotation(0,0);
    }

    void UpdateRotation(float precentage, float targetSpeed)
    {
        throttle.localEulerAngles = Vector3.Lerp(rotationMin, rotationMax, precentage);
    }
}
