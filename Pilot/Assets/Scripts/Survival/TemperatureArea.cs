using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureArea : MonoBehaviour
{
    [SerializeField] private float localTemperature;
    [SerializeField] private float defaultTemperature;
    
    void OnTriggerEnter(Collider other)
    {
        HeatConductor hc = other.GetComponent<HeatConductor>();
        if(hc)
        {
            hc.SetAreaTemperature(localTemperature);
        }
    }

    void OnTriggerExit(Collider other)
    {
        HeatConductor hc = other.GetComponent<HeatConductor>();
        if(hc)
        {
            hc.SetAreaTemperature(defaultTemperature);
        }
    }
}
