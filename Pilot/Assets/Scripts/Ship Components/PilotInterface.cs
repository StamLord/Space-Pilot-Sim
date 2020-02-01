using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotInterface : MonoBehaviour
{

    public delegate bool AccelerateDelegate();
    public event AccelerateDelegate OnAccelerate;

    public delegate bool DeccelerateDelegate();
    public event DeccelerateDelegate OnDeccelerate;

    public delegate bool RollDelegate(float roll);
    public event RollDelegate OnRoll;

    public delegate bool PitchDelegate(float pitch);
    public event PitchDelegate OnPitch;

    public delegate bool BrakeDelegate();
    public event BrakeDelegate OnBrake;

    public delegate void CommunicationDelegate();
    public event CommunicationDelegate OnCommunication;

    public delegate void LockTargetDelegate();
    public event LockTargetDelegate OnLockTarget;

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
        
        if(OnRoll != null)
            OnRoll(Input.GetAxis("Horizontal"));

        if(OnPitch != null)
            OnPitch(Input.GetAxis("Vertical"));

        if(Input.GetKey(KeyCode.Space))
        {
            if(OnBrake != null)
                OnBrake();
        }

        if(Input.GetKeyDown(KeyCode.C))
            if(OnCommunication != null) OnCommunication();

        if(Input.GetKeyDown(KeyCode.T))
            if(OnLockTarget != null) OnLockTarget();

    }
}
