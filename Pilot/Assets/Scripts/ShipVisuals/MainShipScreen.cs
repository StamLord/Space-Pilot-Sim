using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainShipScreen : MonoBehaviour
{
    [SerializeField] private PilotInterface pi;

    [SerializeField] private GameObject flightInstructions;

    [Header("Main View")]
    [SerializeField] private GameObject mainView;
    [SerializeField] private TextMeshProUGUI brakeText;
    [SerializeField] private float hideBrakeTextAfter = .1f;
    [SerializeField] private TextMeshProUGUI coordinatesText;
    
    private Color brakeTextColor;
    private Color brakeTextColorHidden;

    private float lastBrake;
    private void Awake() 
    {
        if(pi != null)
        {
            pi.OnShowFlightInstructions += ShowInstructions;
            pi.OnBrake += ShowBrakeText;
        }

        brakeTextColor = brakeText.color;
        brakeTextColorHidden = new Color(brakeTextColor.r, brakeTextColor.g, brakeTextColor.b, 0f);
    }

    public void ShowInstructions()
    {
        flightInstructions.SetActive(!flightInstructions.activeSelf);
        mainView.SetActive(!flightInstructions.activeSelf);
    }

    private void ShowBrakeText() 
    {
        brakeText.color = brakeTextColor;
        lastBrake = Time.time;
    }

    private void HideBrakeText() 
    {
        brakeText.color = brakeTextColorHidden;
    }

    private void UpdateCoordinates(Vector3 position)
    {
        if(coordinatesText == null) return;
        decimal x = Utility.FloatToDecimal(position.x, 6);
        decimal y = Utility.FloatToDecimal(position.y, 6);
        decimal z = Utility.FloatToDecimal(position.z, 6);
        coordinatesText.text = string.Format("X:{0}\nY:{1}\nZ:{2}", x, y, z);
    }

    private void Update() 
    {
        if(Time.time - lastBrake > hideBrakeTextAfter)
            HideBrakeText();
        
        if(FloatingPointSolution.instance != null)
            UpdateCoordinates(FloatingPointSolution.instance.GetSimulatedSpacePosition(transform.position));
    }
}
