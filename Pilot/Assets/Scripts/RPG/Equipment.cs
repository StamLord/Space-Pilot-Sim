using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public EquipmentType equipmentType {get; private set;}

    public Equipment(string name, string description, int cost, Mesh model, EquipmentType equipmentType) : base(name, description, cost, model)
    {
        this.equipmentType = equipmentType;
    }
}

public enum EquipmentType 
{
    SUIT,
    HELMET,
    TORSO,
    LEGS,
    HANDS,
    FEET
}