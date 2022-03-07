using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenArea : MonoBehaviour
{
    [SerializeField] private bool oxygen;
    public bool Oxygen {get{return oxygen;}}
    
    void OnTriggerEnter(Collider other)
    {
        Breather br = other.GetComponent<Breather>();
        if(br)
        {
            br.EnterOxygenArea(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Breather br = other.GetComponent<Breather>();
        if(br)
        {
            br.ExitOxygenArea(this);
        }
    }
}
