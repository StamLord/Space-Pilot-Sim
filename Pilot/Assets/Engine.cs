using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Component
{
    public Rigidbody rb;
    public PilotInterface pi;

    public float maximumSpeed = 200;
    public float acceleration;
    [Range(0,1)]
    private float _power = 0;
    private float power {
        get{return this._power;}
        set{this._power = Mathf.Clamp(value, 0, 1);}
    }

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnAccelerate += Accelerate;
            pi.OnDeccelerate += Decelerate;
        }
        
    }
    
    public bool Accelerate()
    {
        if(functional == false) return false;

        power += acceleration * Time.deltaTime;
        rb.velocity = transform.forward * power * maximumSpeed;

        return true;
    }

    public bool Decelerate()
    {
        if(functional == false) return false;

        power -= acceleration * Time.deltaTime;

        return true;
    }

    void FixedUpdate()
    {
        power -= acceleration / 2 * Time.deltaTime;
        // rb.AddForce(transform.forward * maximumSpeed * power, ForceMode.Force);
        Debug.Log(rb.angularVelocity);
    }

}
