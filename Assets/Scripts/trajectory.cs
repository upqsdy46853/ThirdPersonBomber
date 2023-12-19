using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class trajectory : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    LineRenderer lineRenderer;
    public float numPoints = 50.0f;
    public float timeBetweenPoints = 0.1f;
    public float F = 5.0f;
    public LayerMask CollidableLayers;
    Camera mainCamera;
    public GameObject landingPos;
    [Networked] private TickTimer delay { get; set; }
    Vector3 startingVelocity;
    public Animator a;



    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
        DontDestroyOnLoad(mainCamera);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startingPosition = this.transform.position;
        float cam_pitch = mainCamera.transform.rotation.eulerAngles.x;
        float pitch = -40;
        pitch += cam_pitch;
        transform.rotation = mainCamera.transform.rotation;
        transform.Rotate(pitch, 0, 0);

        startingVelocity = transform.forward * F;
        
        List<Vector3> points = new List<Vector3>();
        for (float t = 0; t < numPoints; t += timeBetweenPoints){
            Vector3 newPoint = startingPosition + t * startingVelocity;
            newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y/2f * t * t; 
            points.Add(newPoint);
            if(Physics.OverlapSphere(newPoint, 0.1f, CollidableLayers).Length > 0){
                lineRenderer.positionCount = points.Count;
                landingPos.transform.position = newPoint;
                break;
            }
        }
        lineRenderer.SetPositions(points.ToArray());


    }
    
    public override void FixedUpdateNetwork(){
        if (GetInput(out NetworkInputData data)){
            if (delay.ExpiredOrNotRunning(Runner))
            {
                if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 2.0f);
                    Runner.Spawn(bullet,
                    transform.position,
                    Quaternion.identity,
                    Object.InputAuthority,
                    (runner, o) =>
                    {
                        o.GetComponent<bullet>().Init(data.startingVelocity);
                        //a.SetBool( "attack", true );
                    });
                    a.SetBool( "attack", true );
                }

                else
                {
                    //a.SetBool( "attack", false );

                }

            }

            else
            {
                a.SetBool( "attack", false );

            }

        }
    }

    public override void Spawned(){
        if (!HasInputAuthority){
            lineRenderer.enabled = false;
            landingPos.SetActive(false);
        }
    }

    public Vector3 GetStartingVelocity()
    {
        return startingVelocity;
    }
    
}
