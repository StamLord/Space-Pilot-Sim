using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brakes : Component
{
    public PilotInterface pi;
    public Rigidbody rb;
    public Engine engine;

    public float brakeForce;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(engine == null)
            engine = GetComponent<Engine>();

        if(pi != null)
        {
            pi.OnBrake += Brake;
        }
    }

    void Brake()
    {
        if(functional == false) return;

        engine.Deccelerate();
        rb.AddForce(-brakeForce * rb.velocity);
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        Debug.DrawRay(transform.position, -brakeForce * rb.velocity, Color.blue, 1);
    }
}
