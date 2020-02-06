using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatConductor : MonoBehaviour
{
    [SerializeField] 
    private float areaTemperature = 0;
    [SerializeField] 
    private float bodyTemperature = 37;
    [Tooltip("Temperature delta will be multiplied by this")][SerializeField] 
    private float temperatureChangeMult = .05f;
    [Tooltip("Min temperature change per second")][SerializeField] 
    private float minTemperatureChangeRate = .1f;
    [Tooltip("Max temperature change per second")][SerializeField] 
    private float maxTemperatureChangeRate = .5f;
    private float lastTempChange;

    public delegate void areaTempChangeDelegate(float temperature);
    public event  areaTempChangeDelegate onAreaTempChange;

    public delegate void bodyTempChangeDelegate(float temperature);
    public event  bodyTempChangeDelegate onBodyTempChange;

    void Update()
    {
        AdjustTemperature();
    }

    void AdjustTemperature()
    {
        // Updates every second
        if(Time.time - lastTempChange < 1) return;

        float delta = areaTemperature - bodyTemperature;
        if(delta == 0) return;

        delta *= temperatureChangeMult;

        int sign = (delta < 0)? -1 : 1;
        delta = Mathf.Clamp(Mathf.Abs(delta), minTemperatureChangeRate, maxTemperatureChangeRate);
        delta *= sign;

        // Avoids cases of going back and forth missing the target temperature
        if(delta < 0 && bodyTemperature + delta < areaTemperature ||
            delta > 0 && bodyTemperature + delta > areaTemperature) 
            bodyTemperature = areaTemperature;
        else
            bodyTemperature += delta;

        if(onBodyTempChange != null) onBodyTempChange(bodyTemperature);

        lastTempChange = Time.time;
    }
    
    public void SetAreaTemperature(float temperature)
    {
        areaTemperature = temperature;
        if(onAreaTempChange != null) onAreaTempChange(areaTemperature);
    }
}
