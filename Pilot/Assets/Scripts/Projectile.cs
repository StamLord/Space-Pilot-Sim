using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    Rigidbody[] ignore;
    int damage = 25;
    float force = .1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void InitializeProjectile(Vector3 velocity, float relativeSpeed, Rigidbody[] ignore)
    {
        rb.velocity = transform.forward * relativeSpeed + velocity;
        this.ignore = ignore;
    }

    void OnTriggerEnter(Collider other)
    {
        foreach(Rigidbody r in ignore)
        {
            if (r == other.attachedRigidbody)
                return;
        }

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if(damagable != null)
        {
            damagable.Damage(damage);
        }

        if(other.attachedRigidbody)
        {
            other.attachedRigidbody.AddForce(rb.velocity.normalized * force, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
