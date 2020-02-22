using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : Item
{
    public WeaponType weaponType { get; private set;}
    public AmmoType ammoType { get; private set;}
    public int magazineSize {get; private set;}
    public int magazine {get; private set;}
    public float fireRate {get; private set;}
    public int bulletsPerShot {get; private set;}
    public int damage {get; private set;}


    public Weapon(string name, string description, int cost, Mesh model, WeaponType weaponType, AmmoType ammoType, int magazineSize, int bulletsPerShot, int damage) : base(name, description, cost, model)
    {
        this.weaponType = weaponType;
        this.ammoType = ammoType;
        this.magazineSize = magazineSize;
        this.bulletsPerShot = bulletsPerShot;
        this.damage = damage;
        Reload();
    }

    public void Reload()
    {
        magazine = magazineSize;
    }

    public bool Shoot()
    {
        if(magazine < bulletsPerShot)
            return false;
        magazine -= bulletsPerShot;
        return true;
    }
}

[System.Serializable]
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

[System.Serializable]
public enum AmmoType
{
    KINETIC,
    ENERGY
}
