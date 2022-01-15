using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private Character character;
    private SurvivalSystem survivalSystem;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image hungerBar;
    [SerializeField] private Image thirstBar;
    [SerializeField] private Image oxygenBar;
    [SerializeField] private TextMeshProUGUI breathingText;
    [SerializeField] private TextMeshProUGUI temperature;

    private float lastOxygenValue;

    void Awake()
    {
        character = playerObject.GetComponent<Character>();
        survivalSystem = playerObject.GetComponent<SurvivalSystem>();

        if(character)
        {
            character.OnHealthChange += HealthUpdate;
        }

        if(survivalSystem)
        {
            survivalSystem.OnHungerChange += HungerUpdate;
            survivalSystem.OnThirstChange += ThirstUpdate;
            survivalSystem.OnOxygenChange += OxygenUpdate;
            survivalSystem.OnBodyTempChange += TemperatureUpdate;
        }
    }

    void HealthUpdate(int health, int maxHealth)
    {
        // healthBar.fillAmount = health / maxHealth;
        healthBar.rectTransform.localScale = new Vector3((float)health/(float)maxHealth, 1, 1);
    }

    void HungerUpdate(float hunger)
    {
        //hungerBar.fillAmount = hunger;
        hungerBar.rectTransform.localScale = new Vector3(1, hunger, 1);
    }

    void ThirstUpdate(float thirst)
    {
        // thirstBar.fillAmount = thirst;
        thirstBar.rectTransform.localScale = new Vector3(1, thirst, 1);
    }
    
    void OxygenUpdate(float oxygen)
    {
        oxygen = Mathf.Clamp(oxygen, 0, 1);
        oxygenBar.rectTransform.localScale = new Vector3(oxygen, 1, 1);

        if(oxygen >= 1)
            oxygenBar.color = new Color(oxygenBar.color.r,oxygenBar.color.g,oxygenBar.color.b,0);
        else
        {
            oxygenBar.color = new Color(oxygenBar.color.r,oxygenBar.color.g,oxygenBar.color.b,1);
            float isBreathing = (oxygen < lastOxygenValue)? 1f : 0f; // If loosing oxygen since last update 0
            breathingText.color = new Color(breathingText.color.r, breathingText.color.b, breathingText.color.g, isBreathing);
        }
        lastOxygenValue = oxygen;
    }

    void TemperatureUpdate(float temp)
    {
        temperature.text = Utility.FloatToDecimal(temp, 2) + "c";
    }
}