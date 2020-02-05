using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strafing : Component
{
    public PilotInterface pi;
    public Rigidbody rb;

    public float horizontalForce = 10f;
    public float verticalForce = 10f;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnHorizontalStrafe += Horizontal;
            pi.OnVerticalStrafe += Vertical;
        }
    }

    void Horizontal(float horizontal)
    {
        if(functional == false || rb == null) return;
        rb.AddRelativeForce(Vector3.right * horizontal * horizontalForce, ForceMode.Force);

    }

    void Vertical(float vertical)
    {
        if(functional == false || rb == null) return;
        rb.AddRelativeForce(Vector3.up * vertical * horizontalForce, ForceMode.Force);
    }

}
