using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalSystem : MonoBehaviour
{
    private Character character;
    private HeatConductor heat;
    private Breather breath;

    public float freezingTemp = 22;
    public float overheatTemp = 40;
    public float noAirTime = 90;
    [Tooltip("Hunger depletion per hour")]
    public float hungerDepleteRate = .1f;
    [Tooltip("Thirst depletion per hour")]
    public float thirstDepleteRate = .05f;
    
    public int freezingDamage = 1;
    public int overheatDamage = 1;
    public int suffocatingDamage = 5;
    public int starvingDamage = 1;
    public int dehydratingDamage = 1;

    [Range(0,1)]
    public float hunger = 1;
    [Range(0,1)]
    public float thirst = 1;
    

    private bool isFreezing;
    private bool isOverHeating;
    private bool isSuffocating;
    private bool isStarving;
    private bool isDehydrating;


    private float lastSurvivalUpdate;
    private float survivalUpdateRate = 1; // In seconds

    void Awake()
    {
        if(heat == null)
            heat = GetComponent<HeatConductor>();
        if(heat)
        {
            heat.onBodyTempChange += BodyTemperatureChange;
        }

        if(breath == null)
            breath = GetComponent<Breather>();
        if(breath)
        {
            breath.onSuffocate += Suffocate;
        }
    }

    void Update()
    {
        SurvivalUpdate();
    }
    
    void BodyTemperatureChange(float temperature)
    {
        isFreezing = (temperature < freezingTemp);
        isOverHeating = (temperature > overheatTemp);
    }

    void Suffocate(float duration)
    {
        isSuffocating = (duration >= noAirTime);
    }

    void SurvivalUpdate()
    {
        if(Time.time - lastSurvivalUpdate < survivalUpdateRate)
            return;

        HungerUpdate();
        ThirstUpdate();        

        if(isFreezing)
            FreezingEffects();
            
        if(isOverHeating)
            OverheathingEffects();

        if(isSuffocating)
            SuffocatingEffects();

        if(isStarving)
            StarvingEffects();
        
        if(isDehydrating)
            DehydratingEffects();

        lastSurvivalUpdate = Time.time;
    }

    void HungerUpdate()
    {
        float updatedPerHour = 3600 / survivalUpdateRate;
        hunger -= hungerDepleteRate / updatedPerHour;

        hunger = Mathf.Clamp(hunger, 0, 1);

        if(hunger == 0)
            isStarving = true;
        else
            isStarving = false;
    }

    void ThirstUpdate()
    {
        float updatedPerHour = 3600 / survivalUpdateRate;
        thirst -= thirstDepleteRate / updatedPerHour;

        thirst = Mathf.Clamp(thirst, 0, 1);

        if(thirst == 0)
            isDehydrating = true;
        else
            isDehydrating = false;
    }

    void FreezingEffects()
    {
        Debug.Log("Freezing");
        // Slow Movement
        // Deplete Health
        character.Damage(freezingDamage);
    }

    void OverheathingEffects()
    {
        Debug.Log("Overheating");
        // Dizziness
        // Slow Movement
        // Deplete Health
        character.Damage(overheatDamage);
        // Increase Thirst
    }

    void SuffocatingEffects()
    {
        Debug.Log("Suffocating");
        // Tunnel Vision
        // Deplete Health
        character.Damage(suffocatingDamage);
    }

    void StarvingEffects()
    {
        // Deplete Health
        character.Damage(starvingDamage);
    }

    void DehydratingEffects()
    {
        // Deplete Health
        character.Damage(dehydratingDamage);
    }
}
