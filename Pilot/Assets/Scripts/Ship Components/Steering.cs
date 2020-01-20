using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : Component
{
    public PilotInterface pi;
    public Rigidbody rb;

    public float rollForce = 10f;
    public float pitchForce = 10f;
    public float yawForce = 10f;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnRoll += Roll;
            pi.OnPitch += Pitch;
            pi.OnYaw += Yaw;
        }
    }

    void Roll(float roll)
    {
        if(functional == false || rb == null) return;

        rb.AddTorque(rollForce * transform.forward * -roll, ForceMode.Force);
    }

    void Pitch(float pitch)
    {
        if(functional == false || rb == null) return;

        rb.AddTorque(pitchForce * transform.right * pitch, ForceMode.Force);
    }

    void Yaw(float yaw)
    {
        if(functional == false || rb == null) return;

        rb.AddTorque(yawForce * transform.up * yaw, ForceMode.Force);
    }

}
