using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGauge : Component
{
    public Text speedText;
    public Engine engine;

    void Awake()
    {
        if(engine == null)
            engine = GetComponent<Engine>();
        
        if(engine != null)
            engine.OnSpeedChange += UpdateSpeedText;
    }

    void UpdateSpeedText(float value)
    {
        if(speedText != null)
            speedText.text = ((int)value).ToString();
    }
}
