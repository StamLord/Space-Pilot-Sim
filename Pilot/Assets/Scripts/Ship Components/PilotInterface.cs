using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotInterface : MonoBehaviour
{

    public delegate void AccelerateDelegate();
    public event AccelerateDelegate OnAccelerate;

    public delegate void DeccelerateDelegate();
    public event DeccelerateDelegate OnDeccelerate;

    public delegate void RollDelegate(float roll);
    public event RollDelegate OnRoll;

    public delegate void PitchDelegate(float pitch);
    public event PitchDelegate OnPitch;

    public delegate void YawDelegate(float yaw);
    public event YawDelegate OnYaw;

    public delegate void BrakeDelegate();
    public event BrakeDelegate OnBrake;
    
    public delegate void SetThrottleDelegate(float precentage);
    public event SetThrottleDelegate OnSetThrottle;
    
    public delegate void HorizontalStrafeDelegate(float horizontal);
    public event HorizontalStrafeDelegate OnHorizontalStrafe;

    public delegate void VerticalStrafeDelegate(float vertical);
    public event VerticalStrafeDelegate OnVerticalStrafe;

    public delegate void MousePilotingDelegate(bool state);
    public event MousePilotingDelegate OnMousePiloting;

    public delegate void Fire1Delegate();
    public event Fire1Delegate OnFire1;

    public delegate void OreMagnetDelegate();
    public event OreMagnetDelegate OnOreMagnet;

    bool rightClick;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {   
            rightClick = true;
            Cursor.lockState = CursorLockMode.Locked;
            if(OnMousePiloting != null) OnMousePiloting(rightClick);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            rightClick = false;
            Cursor.lockState = CursorLockMode.None;
            if(OnMousePiloting != null) OnMousePiloting(rightClick);
        }

        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(OnFire1 != null)
                OnFire1();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            if(OnOreMagnet != null)
                OnOreMagnet();
        }
    }

    void FixedUpdate()
    {
        // Throttle
        if(Input.GetKey(KeyCode.W))
        {
            if(OnAccelerate != null) OnAccelerate();
        } 
        else if(Input.GetKey(KeyCode.S))
        {
            if(OnDeccelerate != null) OnDeccelerate();
        }

        // Strafe
        if(Input.GetKey(KeyCode.Q))
        {
            if(OnHorizontalStrafe != null) OnHorizontalStrafe(-1);
        } 
        else if(Input.GetKey(KeyCode.E))
        {
            if(OnHorizontalStrafe != null) OnHorizontalStrafe(1);
        }

        if(Input.GetKey(KeyCode.Z))
        {
            if(OnVerticalStrafe != null) OnVerticalStrafe(-1);
        } 
        else if(Input.GetKey(KeyCode.X))
        {
            if(OnVerticalStrafe != null) OnVerticalStrafe(1);
        }


        // Yaw
        if(Input.GetKey(KeyCode.A))
        {
            if(OnYaw != null) OnYaw(-1);
        } 
        else if(Input.GetKey(KeyCode.D))
        {
            if(OnYaw != null) OnYaw(1);
        }
        
        // Roll
        if(OnRoll != null)
        {
            if(rightClick) 
                OnRoll(Input.GetAxis("Mouse X"));
            else
                OnRoll(Input.GetAxis("Horizontal"));
        }

        // Pitch
        if(OnPitch != null)
        {
            if(rightClick) 
                OnPitch(Input.GetAxis("Mouse Y"));
            else
                OnPitch(Input.GetAxis("Vertical"));
        }

        if(Input.GetKey(KeyCode.Space))
        {
            if(OnBrake != null)
                OnBrake();
        }

    }

    public void HiJack(float setThrottle)
    {
        Debug.Log("Hijacked");
        if(OnSetThrottle != null)
            OnSetThrottle(setThrottle);
    }
}
