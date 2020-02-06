using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenArea : MonoBehaviour
{
    [SerializeField] private bool localOxygen;
    [SerializeField] private bool defaultOxygen;
    
    void OnTriggerEnter(Collider other)
    {
        Breather br = other.GetComponent<Breather>();
        if(br)
        {
            br.SetBreathing(localOxygen);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Breather br = other.GetComponent<Breather>();
        if(br)
        {
            br.SetBreathing(defaultOxygen);
        }
    }
}
