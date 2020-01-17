using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrottleGauge : Component
{
    public Text throttleText;
    public Text targetSpeedText;
    public Engine engine;

    void Awake()
    {
        if(engine == null)
            engine = GetComponent<Engine>();
        
        if(engine != null)
            engine.OnThrottleChange += UpdateThrottleText;
    }

    void UpdateThrottleText(float throttle, float targetSpeed)
    {
        if(throttleText != null)
            throttleText.text = Mathf.FloorToInt(throttle * 100).ToString();

        if(targetSpeedText != null)
            targetSpeedText.text = Mathf.FloorToInt(targetSpeed).ToString();
    }
}
