using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabalizers : Component
{
    public Rigidbody rb;

    public float baseAngularDrag = 0.05f;
    public float angularDrag = 100f;

    [Range(0, 0.1f)]
    public float verticalStab = 0.02f;
    [Range(0, 0.1f)]
    public float horizontalStab = 0.01f;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(rb != null)
            rb.angularDrag = baseAngularDrag;
    }

    void FixedUpdate()
    {   
        if(functional == false || rb == null)
            return;

        ApplyAngularStabilization();
        ApplyVerticalStabilization();
        ApplyHorizontalStabilization();
    }

    void ApplyAngularStabilization()
    {
        rb.angularDrag = Mathf.Max(angularDrag * rb.angularVelocity.magnitude, baseAngularDrag);
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
