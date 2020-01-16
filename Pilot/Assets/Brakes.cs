using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brakes : Component
{
    public PilotInterface pi;
    public Rigidbody rb;

    public float brakeForce;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnBrake += Brake;
        }
    }

    bool Brake()
    {
        if(functional == false) return false;

        rb.AddForce(-brakeForce * rb.velocity);

        return true;
    }
}
