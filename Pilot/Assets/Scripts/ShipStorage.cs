using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStorage : MonoBehaviour
{
    public float metalCapacity = 100;
    public float metalStored;

    public delegate void metalUpdatedDelegate(float amount);
    public event metalUpdatedDelegate OnMetalUpdated;

    public bool AddMetal(float amount)
    {
        if(metalStored + amount <= metalCapacity)
        {
            metalStored += amount;
            if(OnMetalUpdated != null) OnMetalUpdated(metalStored);
            return true;
        }
        else
            return false;
    }
}
