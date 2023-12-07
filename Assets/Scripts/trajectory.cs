using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class trajectory : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public GameObject landingPos;

    LineRenderer lineRenderer;
    public float numPoints = 50.0f;
    public float timeBetweenPoints = 0.1f;
    public float F = 5.0f;
    public LayerMask CollidableLayers;
    Camera mainCamera;
    Vector3 startingVelocity;
    private bool _shootPressed;


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _shootPressed = true;
        }

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
        if (HasStateAuthority == false){
            return;
        }

        if(_shootPressed){
            Runner.Spawn(
                bullet, 
                transform.position, 
                Quaternion.identity, 
                Object.InputAuthority,
                (runner, o) => {
                    o.GetComponent<bullet>().Init(startingVelocity);
                }
            );
        }

        _shootPressed = false;
    }

    public override void Spawned(){
        if (!HasStateAuthority){
            lineRenderer.enabled = false;
            landingPos.SetActive(false);
            gameObject.GetComponent<trajectory>().enabled = false;
        }
    }
}
