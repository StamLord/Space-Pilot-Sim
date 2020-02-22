using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons = new Weapon[5];
    [SerializeField] private int current;
    [SerializeField] private bool holstered;
    [SerializeField] private MeshFilter weaponModel;
    private new Camera camera;

    void Awake()
    {
        camera = Camera.main;
    }

    void Start()
    {
        Mesh gun = Resources.Load<Mesh>("WM_SciFi_Weapon1_Lite/Meshes/WM_SF_Wp01_a");
        Mesh knife = Resources.Load<Mesh>("M9_Knife/M9_Knife");

        weapons[0] = new Weapon("Knife", "You do not run faster when holding it. Trust us.", 80, knife, WeaponType.KNIFE, AmmoType.KINETIC, 0, 0, 10);
        weapons[1] = new Weapon("Laser Gun", "Must have for any would be Spacer.", 600, gun, WeaponType.HANDGUN, AmmoType.KINETIC, 16, 1, 20);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeCurrent(0);
        }

        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeCurrent(1);
        }

        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeCurrent(2);
        }

        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeCurrent(3);
        }

        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeCurrent(4);
        }
    }

    void ChangeCurrent(int slot)
    {
        if(slot < 0 || slot >= weapons.Length || weapons[slot].model == null)
            return;

        if(current == slot)
        {
            DrawWeapon(holstered);
            return;
        }

        current = slot;
        DrawWeapon(true);
    }

    void DrawWeapon(bool draw)
    {
        holstered = !draw;
        if(draw)
        {
            // Animate Draw
            weaponModel.mesh = weapons[current].model;
            // Update UI
        }
        else
        {
            // Animate Holster
            weaponModel.mesh = null;
            // Update UI
        }
    }

    void Shoot()
    {
        Weapon weapon = weapons[current];

        if(weapon.weaponType == WeaponType.KNIFE)
            Debug.Log("MELEE");
        else
        {
            // Removes ammo if able or returns
            if(weapon.Shoot() == false) 
                return;
            
            // Update UI

            // Instantiate Projectile or Hitscan
            switch(weapon.ammoType)
            {
                case AmmoType.ENERGY:
                    // Instantiate Projectile
                    break;
                case AmmoType.KINETIC:
                    RaycastHit hit;
                    LayerMask mask = LayerMask.GetMask("Default", "Interior");
                    if(Physics.Raycast(camera.ScreenPointToRay(new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2)), out hit, 100f, mask, QueryTriggerInteraction.Ignore))
                    {   Debug.Log("HIT " + hit.collider.gameObject);
                        IDamagable damagable = hit.collider.GetComponent<IDamagable>();
                        if(damagable != null)
                            damagable.Damage(weapon.damage);

                        Rigidbody rigid = hit.collider.GetComponent<Rigidbody>();
                        if(rigid)
                        {
                            Debug.Log("RIGID");
                            rigid.AddForce((hit.point - transform.position).normalized, ForceMode.VelocityChange);
                        }
                    }

                    break;
            }
        }
    }
}
