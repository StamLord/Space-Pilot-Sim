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

    public delegate void MousePilotingDelegate(bool state);
    public event MousePilotingDelegate OnMousePiloting;

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
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W))
        {
            if(OnAccelerate != null) OnAccelerate();
        } 
        else if(Input.GetKey(KeyCode.S))
        {
            if(OnDeccelerate != null) OnDeccelerate();
        }

        if(Input.GetKey(KeyCode.A))
        {
            if(OnYaw != null) OnYaw(-1);
        } 
        else if(Input.GetKey(KeyCode.D))
        {
            if(OnYaw != null) OnYaw(1);
        }
        
        if(OnRoll != null)
        {
            if(rightClick) 
                OnRoll(Input.GetAxis("Mouse X"));
            else
                OnRoll(Input.GetAxis("Horizontal"));
        }

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
}
