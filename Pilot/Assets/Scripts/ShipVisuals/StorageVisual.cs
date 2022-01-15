using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StorageVisual : MonoBehaviour
{
    public ShipStorage storage;
    public TextMeshProUGUI metalDisplay;

    void Awake()
    {
        if(storage == null)
            storage = GetComponent<ShipStorage>();

        storage.OnMetalUpdated += UpdateMetalVisual;
    }

    void UpdateMetalVisual(float amount)
    {
        if(metalDisplay == null)
        {
            Debug.LogWarning("No reference for metalDisplay on StorageVisual");
            return;
        }

        metalDisplay.text = "" + double.Parse(amount.ToString());
    }
}
