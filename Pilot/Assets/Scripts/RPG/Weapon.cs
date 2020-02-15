using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public WeaponType weaponType { get; private set;}
    public AmmoType ammoType { get; private set;}

    public Weapon(string name, string description, int cost, WeaponType weaponType, AmmoType ammoType) : base(name, description, cost)
    {
        this.weaponType = weaponType;
        this.ammoType = ammoType;
    }
}

public enum WeaponType
{
    KNIFE,
    HANDGUN,
    SHOTGUN,
    RIFLE,
    SNIPER,
    RPG,
    GRENADE
}

public enum AmmoType
{
    KINETIC,
    ENERGY
}
