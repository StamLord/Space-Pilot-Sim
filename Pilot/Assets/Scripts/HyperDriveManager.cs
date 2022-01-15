using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HyperDriveManager : MonoBehaviour
{
    [SerializeField] private float chargeTime = 5;
    [SerializeField] private float timeInHyperSpace = 10;
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private Camera[] cameras;
    private float[] fovs;

    [SerializeField] private float targetFov = 160;

    private float tranTimer;
    
    private GameObject ship;
    private Vector3 exitPosition;

    #region  Singleton
    
    public static HyperDriveManager instance;
    void Awake()
    {
        if(instance)
        {
            Debug.LogWarning("More than 1 instance of HypderDriveManager exists!");
            Destroy(this.gameObject);
        } 
        else
            instance = this;
    }
    #endregion

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        fovs = new float[cameras.Length];
        for (var i = 0; i < cameras.Length; i++)
            fovs[i] = cameras[i].fieldOfView;
    }

    public void EnterHyperSpace(GameObject ship, Vector3 exitPosition)
    {
        DontDestroyOnLoad(ship);
        DontDestroyOnLoad(ship.GetComponent<ShipInterior>().GetInterior.gameObject);
        DontDestroyOnLoad(Camera.main);

        this.ship = ship;
        this.exitPosition = exitPosition;
        HyperDrive();
    }
    
    public void EnterHyperSpace(GameObject ship, Vector3 exitPosition, Material transition)
    {
        EnterHyperSpace(ship, exitPosition);
        StartCoroutine("TransitionEffect", transition);
    }

    private void HyperDrive()
    {
        StartCoroutine("CameraEffect", false);
        StartCoroutine("ExitHyperSpace");
    }

    IEnumerator CameraEffect(bool reversed = false)
    {
        float timer = 0;
        while(chargeTime > timer)
        {
            for(int i=0; i<cameras.Length; i++)
                if(reversed)
                    cameras[i].fieldOfView = Mathf.Lerp(targetFov, fovs[i], timer / chargeTime);
                else
                    cameras[i].fieldOfView = Mathf.Lerp(fovs[i], targetFov, timer / chargeTime);

            yield return null;
            timer += Time.deltaTime;
        }
        //SceneManager.LoadScene("HyperSpace", LoadSceneMode.Single);
    }

    private void ParticlesEffect(bool on)
    {
        foreach(ParticleSystem par in particles)
        {
            if(on)
            {
                par.Play();
                par.startSpeed = 100;
            }
            else
                par.Stop();
        }
    }

    IEnumerator TransitionEffect(Material transition)
    {
        while(chargeTime * 2 > tranTimer)
        {
            Color final = Color.Lerp(Color.black, Color.white, Mathf.PingPong(tranTimer / chargeTime, 1));
            transition.SetColor("_EmissionColor", final);
            
            yield return null;
            tranTimer += Time.deltaTime;
        }

        tranTimer = 0;
        transition.SetColor("_EmissionColor", Color.black);
    }

    IEnumerator ExitHyperSpace()
    {
        float totalTime = 0;
        ParticlesEffect(true);

        while(totalTime < timeInHyperSpace + chargeTime)
        {
            yield return null;
            totalTime += Time.deltaTime;
        }
        
        ParticlesEffect(false);
        StartCoroutine("CameraEffect", true);

        //SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        ship.transform.position = exitPosition;
    }
}
