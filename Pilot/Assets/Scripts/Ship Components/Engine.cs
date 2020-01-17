using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Component
{
    public Rigidbody rb;
    public PilotInterface pi;

    [Space]
    public float maximumSpeed = 200;
    public float acceleration;
    public float decceleration;
    [Range(0,1)][SerializeField]
    private float _throttle = 0;
    private float throttle {
        get{return this._throttle;}
        set{this._throttle = Mathf.Clamp(value, 0, 1);}
    }

    private float currentSpeed = -1;

    public delegate void SpeedChangeDelegate(float value);
    public event SpeedChangeDelegate OnSpeedChange;

    public delegate void ThrottleChangeDelegate(float throttle, float targetSpeed);
    public event ThrottleChangeDelegate OnThrottleChange;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnAccelerate += Accelerate;
            pi.OnDeccelerate += Deccelerate;
        }
        
    }
    
    public bool Accelerate()
    {
        if(functional == false) return false;

        throttle += acceleration * Time.deltaTime;
        
        if(OnThrottleChange != null)
            OnThrottleChange(throttle, throttle * maximumSpeed);

        return true;
    }

    public bool Deccelerate()
    {
        if(functional == false) return false;

        throttle -= decceleration * Time.deltaTime;

        if(OnThrottleChange != null)
            OnThrottleChange(throttle, throttle * maximumSpeed);

        return true;
    }

    void FixedUpdate()
    {
        Vector3 force = CalculateForce();
        Debug.Log(force);
        rb.AddRelativeForce(force, ForceMode.Force);
        Debug.DrawRay(transform.position, force, Color.red, 1);
    }

    void LateUpdate()
    {
        if(rb.velocity.magnitude != currentSpeed)
        {
            currentSpeed = transform.InverseTransformVector(rb.velocity).z;
            if(OnSpeedChange != null)
                OnSpeedChange(currentSpeed);
        }
    }

    Vector3 CalculateForce()
    {
        float difference = throttle * maximumSpeed - transform.InverseTransformVector(rb.velocity).z;
        return Vector3.forward * difference;
    }

}
