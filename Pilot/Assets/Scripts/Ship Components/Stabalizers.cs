using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabalizers : Component
{
    public Rigidbody rb;
    public PilotInterface pi;

    public float rollingStab = 0.5f;

    [Range(0, 0.1f)]
    public float verticalStab = 0.02f;
    [Range(0, 0.1f)]
    public float horizontalStab = 0.01f;

    public bool isRolling;
    public bool isPitching;
    public bool isYawing;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnRoll += SetRolling;
            pi.OnPitch += SetPitching;
        }
    }

    void FixedUpdate()
    {   
        if(functional == false || rb == null)
            return;

        ApplyAngularStabilization();
        ApplyVerticalStabilization();
        ApplyHorizontalStabilization();
    }

    public void SetRolling(float input)
    {
        isRolling = (input == 0) ? false : true;
    }

    public void SetPitching(float input)
    {
        isPitching = (input == 0) ? false : true;
    }

    void ApplyAngularStabilization()
    {   
        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity);

        if(isRolling == false)
            rb.AddRelativeTorque(new Vector3(0,0,-localAngularVelocity.z) * rollingStab, ForceMode.Impulse);
        if(isPitching == false)
            rb.AddRelativeTorque(new Vector3(-localAngularVelocity.x, 0,0) * rollingStab, ForceMode.Impulse);
        if(isYawing == false)
            rb.AddRelativeTorque(new Vector3(0, -localAngularVelocity.y, 0) * rollingStab, ForceMode.Impulse);

    }

    void ApplyVerticalStabilization()
    {
        Vector3 force = CalculateVerticalForce();
        rb.AddRelativeForce(force, ForceMode.Impulse);
        Debug.DrawRay(transform.position, force * 10, Color.yellow, 1);
    }

    void ApplyHorizontalStabilization()
    {
        Vector3 force = CalculateHorizontalForce();
        rb.AddRelativeForce(force, ForceMode.Impulse);
        Debug.DrawRay(transform.position, force * 10, Color.green, 1);
    }

    Vector3 CalculateVerticalForce()
    {
        float difference = 0 - transform.InverseTransformVector(rb.velocity).y;
        return Vector3.up * difference * verticalStab;
    }

    Vector3 CalculateHorizontalForce()
    {
        float difference = 0 - transform.InverseTransformVector(rb.velocity).x;
        return Vector3.right * difference * horizontalStab;
    }

}
