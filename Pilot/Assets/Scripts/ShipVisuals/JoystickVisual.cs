using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickVisual : MonoBehaviour
{   
    public Transform joystick;
    public Vector2 horizontalMinMax;
    public Vector2 verticalMinMax;

    public float maxHorizontalChange = 0.1f;
    public float maxVerticalChange = 0.1f;

    float lastHorizontalPrecentage = 0.5f;
    float lastVerticalPrecentage = 0.5f;
    
    public PilotInterface pi;

    void Awake()
    {
        if(pi == null)
            pi = GetComponent<PilotInterface>();
        
        if(pi)
        {
            pi.OnRoll += UpdateHorizontal;
            pi.OnPitch += UpdateVertical;
        }

    }

    void UpdateHorizontal(float precentage)
    {
        if(joystick == null) return;

        // Remap value to 0 .. 1
        precentage += 1f;
        precentage /= 2f;

        // Clamp maximum change
        float delta = precentage - lastHorizontalPrecentage;
        delta = Mathf.Clamp(delta, -maxHorizontalChange, maxHorizontalChange);

        joystick.localEulerAngles = Vector3.Lerp(
            new Vector3(joystick.localEulerAngles.x, joystick.localEulerAngles.y, horizontalMinMax.x), 
            new Vector3(joystick.localEulerAngles.y, joystick.localEulerAngles.y, horizontalMinMax.y), 
            lastHorizontalPrecentage + delta);

        // Save for future reference
        lastHorizontalPrecentage = lastHorizontalPrecentage + delta;
    }

    void UpdateVertical(float precentage)
    {
        if(joystick == null) return;
        
        // Remap value to 0 .. 1
        precentage += 1f;
        precentage /= 2f;

        // Clamp maximum change
        float delta = precentage - lastVerticalPrecentage;
        delta = Mathf.Clamp(delta, -maxVerticalChange, maxVerticalChange);

        joystick.localEulerAngles = Vector3.Lerp(
            new Vector3(verticalMinMax.x, joystick.localEulerAngles.y, joystick.localEulerAngles.z), 
            new Vector3(verticalMinMax.y, joystick.localEulerAngles.y, joystick.localEulerAngles.z), 
            lastVerticalPrecentage + delta);

        // Save for future reference
        lastVerticalPrecentage = lastVerticalPrecentage + delta;
    }
}
