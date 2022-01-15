using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperDrive : Component
{
    [SerializeField] private PilotInterface pi;
    [SerializeField] private Renderer hyperTransition;

    [SerializeField] private string destination;
    [SerializeField] private int maxDestinationLength = 9;
    [SerializeField] private bool openConsole;
    
    private float timer;
    
    void Awake()
    {
        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnEngageHyperDrive += EngageHyperDrive;
            pi.OnToggleHyperConsole += ToggleHyperConsole;
            pi.OnStopPiloting += CloseConsole;
        }
    }

    void Update()
    {
        if(openConsole == false) return;

        if(Input.GetKeyDown(KeyCode.Keypad0))
            EnterNum(0);
        else if(Input.GetKeyDown(KeyCode.Keypad1))
            EnterNum(1);
        else if(Input.GetKeyDown(KeyCode.Keypad2))
            EnterNum(2);
        else if(Input.GetKeyDown(KeyCode.Keypad3))
            EnterNum(3);
        else if(Input.GetKeyDown(KeyCode.Keypad4))
            EnterNum(4);
        else if(Input.GetKeyDown(KeyCode.Keypad5))
            EnterNum(5);
        else if(Input.GetKeyDown(KeyCode.Keypad6))
            EnterNum(6);
        else if(Input.GetKeyDown(KeyCode.Keypad7))
            EnterNum(7);
        else if(Input.GetKeyDown(KeyCode.Keypad8))
            EnterNum(8);
        else if(Input.GetKeyDown(KeyCode.Keypad9))
            EnterNum(9);
        else if(Input.GetKeyDown(KeyCode.Backspace))
            DeleteNum();
    }

    void ToggleHyperConsole()
    {
        openConsole = !openConsole;
    }

    void CloseConsole()
    {
        openConsole = false;
    }

    void EnterNum(int num)
    {
        if(destination.Length >= maxDestinationLength)
            return;

        destination += num;
    }

    void DeleteNum()
    {
        if(destination.Length == 0) 
            return;
        
        destination = destination.Substring(0, destination.Length - 1);
    }

    void SetDestination(string newDestination)
    {
        destination = newDestination;
    }
    
    void EngageHyperDrive()
    {
        // Fill
        int missing = maxDestinationLength - destination.Length;
        for(int i = 0; i < missing; i++)
        {
            EnterNum(0);
        }

        CloseConsole();
        Vector3 position = Utility.StringToVector3(destination);
        HyperDriveManager.instance.EnterHyperSpace(gameObject, position, hyperTransition.material);
    }

}
