using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabalizers : Component
{
    public Rigidbody rb;

    public float baseAngularDrag = 0.05f;
    public float angularDrag = 100f;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(rb != null)
            rb.angularDrag = baseAngularDrag;
    }

    void FixedUpdate()
    {
        if(rb != null)
            rb.angularDrag = Mathf.Max(angularDrag * rb.angularVelocity.magnitude, baseAngularDrag);
    }
}
