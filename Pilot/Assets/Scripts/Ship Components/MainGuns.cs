using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGuns : Component
{
    public PilotInterface pi;
    public Rigidbody rb;

    public Transform l_Turret;
    public Transform r_Turret;

    public GameObject projectile;
    public float fireRate = 0.5f;
    public float relativeSpeed= 20;

    float lastShot;

    void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnFire1 += Fire;
        }
    }

    void Fire()
    {
        if(Time.time - lastShot >= fireRate)
        {
            GameObject left = Instantiate(projectile, l_Turret.position, l_Turret.rotation) as GameObject;
            left.GetComponent<Projectile>().InitializeProjectile(
                rb.velocity, relativeSpeed,
                new Rigidbody[] {rb});

            GameObject right = Instantiate(projectile, r_Turret.position, r_Turret.rotation) as GameObject;
            right.GetComponent<Projectile>().InitializeProjectile(
                rb.velocity, relativeSpeed,
                new Rigidbody[] {rb});

            lastShot = Time.time;

        }
    }
}
