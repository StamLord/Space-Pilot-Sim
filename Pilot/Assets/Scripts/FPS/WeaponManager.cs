using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons = new Weapon[5];
    [SerializeField] private int current;
    [SerializeField] private bool holstered;
    
    [Header("Model")]
    [SerializeField] private MeshFilter weaponModel;
    [SerializeField] private Transform barrel;

    [Header("Animation")]
    [SerializeField] private float animationTime = .5f;
    private float animationTimer;
    [SerializeField] private AnimationCurve drawYpos;
    [SerializeField] private Vector3 rotStart = new Vector3(-40f, 0,0);

    [Header("HitScan")]
    [SerializeField] private float hitscanRange = 100f;
    [SerializeField] private float laserLifeTime = 1f;

    private new Camera camera;

    private struct HitscanInfo
    {
        public bool hit;
        public Vector3 hitPoint;
        public Vector3 direction;
    }

    void Awake()
    {
        camera = Camera.main;
    }

    void Start()
    {
        Mesh gun = Resources.Load<Mesh>("WM_SciFi_Weapon1_Lite/Meshes/WM_SF_Wp01_a");
        Mesh knife = Resources.Load<Mesh>("M9_Knife/M9_Knife");
        Mesh rifle = Resources.Load<Mesh>("Content/Meshes/CompactBullpupDMRBody");

        weapons[0] = new Weapon("Knife", "You don't run faster holding it. Trust mre.", 80, knife, WeaponType.KNIFE, AmmoType.KINETIC, 0, 0, 10);
        weapons[1] = new Weapon("Laser Gun", "Must have for any would be Spacer.", 600, gun, WeaponType.HANDGUN, AmmoType.LASER, 16, 1, 20);
        weapons[2] = new Weapon("Rifle", "Cuts through shields like butter.", 2000, rifle, WeaponType.RIFLE, AmmoType.KINETIC, 16, 1, 20);
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
            StartCoroutine("AnimateDraw");
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
                case AmmoType.LASER:
                    HitscanInfo hitscanInfo = Hitscan(weapon.damage);

                    if(LineRendererPool.instance)
                    {
                        LineRenderer lr = LineRendererPool.instance.GetLineFromPool();
                        lr.SetPosition(0, lr.transform.InverseTransformPoint(barrel.position));
                        if(hitscanInfo.hit)
                            lr.SetPosition(1, lr.transform.InverseTransformPoint(hitscanInfo.hitPoint));
                        else
                            lr.SetPosition(1, lr.transform.InverseTransformPoint(hitscanInfo.direction * hitscanRange));
                        
                        StartCoroutine("LaserFade", lr);
                    }
                    else
                        Debug.LogWarning("No linePool ref in " + gameObject.name);
                    break;
                case AmmoType.KINETIC:
                    Hitscan(weapon.damage);
                    break;
                case AmmoType.PLASMA:
                    // Spawn Projectile
                    break;
            }
        }
    }

    IEnumerator AnimateDraw()
    {
        while(animationTime > animationTimer)
        {
            weaponModel.transform.localPosition = new Vector3(0, drawYpos.Evaluate(animationTimer / animationTime), 0);
            weaponModel.transform.localRotation = Quaternion.Euler(Vector3.Lerp(rotStart, Vector3.zero, animationTimer / animationTime));
            yield return null;
            animationTimer += Time.deltaTime;
        }

        animationTimer = 0;
    }
    
    HitscanInfo Hitscan(int damage)
    {
        HitscanInfo hitscanInfo = new HitscanInfo();
        hitscanInfo.direction = camera.transform.forward;

        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Default", "Interior");
        if(Physics.Raycast(camera.ScreenPointToRay(new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2)), out hit, hitscanRange, mask, QueryTriggerInteraction.Ignore))
        {   
            hitscanInfo.hit = true;
            hitscanInfo.hitPoint = hit.point;

            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if(damagable != null)
                damagable.Damage(damage);

            Rigidbody rigid = hit.collider.GetComponent<Rigidbody>();
            if(rigid)
                rigid.AddForce((hit.point - transform.position).normalized, ForceMode.VelocityChange);
        }

        return hitscanInfo;
    }

    IEnumerator LaserFade(LineRenderer lr)
    {
        float death = Time.time + laserLifeTime;
        Color baseStartColor, finalStartColor, baseEndColor, finalEndColor;
        baseStartColor = finalStartColor = lr.startColor;
        baseEndColor = finalEndColor = lr.endColor;

        finalStartColor.a = 0;
        finalEndColor.a = 0;

        while(Time.time <= death)
        {
            float percent = 1 - (death - Time.time) / laserLifeTime;
            lr.startColor = Color.Lerp(baseStartColor, finalStartColor, percent);
            lr.endColor = Color.Lerp(baseEndColor, finalEndColor, percent);
            yield return null;
        }

        lr.startColor = baseStartColor;
        lr.endColor = baseEndColor;
        LineRendererPool.instance.PoolLine(lr);
    }

    // public Weapon Equip(Weapon weapon)
    // {
        
    // }

    // private Weapon Equip(Weapon weapon, int slot)
    // {
        
    // }
    
}
