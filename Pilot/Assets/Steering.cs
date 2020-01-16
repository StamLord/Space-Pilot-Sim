using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : Component
{
    public PilotInterface pi;
    public float rollForce = 10f;
    public float pitchForce = 10f;

    void Awake()
    {
        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnRoll += Roll;
            pi.OnPitch += Pitch;
        }
    }

    bool Roll(float roll)
    {
        if(functional == false) return false;

        transform.Rotate(new Vector3(0,0,rollForce * roll * Time.deltaTime), Space.Self);
        transform.Rotate(new Vector3(0,0,rollForce * roll * Time.deltaTime), Space.Self);
        return true;
    }

    bool Pitch(float pitch)
    {
        if(functional == false) return false;

        transform.Rotate(new Vector3(pitchForce * pitch * Time.deltaTime,0,0), Space.Self);
        transform.Rotate(new Vector3(pitchForce * pitch * Time.deltaTime,0,0), Space.Self);
        return true;
    }

}
