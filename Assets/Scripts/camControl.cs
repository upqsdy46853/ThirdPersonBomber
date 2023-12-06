using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;


public class camControl : NetworkBehaviour
{
    // Start is called before the first frame update
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camLookAt;
    public GameObject CM_vcam;


    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    public override void FixedUpdateNetwork(){
        if (HasStateAuthority == false){
            return;
        }
        camLookAt.localEulerAngles = new Vector3(yAxis.Value, camLookAt.localEulerAngles.y, camLookAt.localEulerAngles.z); 
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
    public override void Spawned(){
        if (HasStateAuthority){
            GameObject playerCamera = Instantiate(CM_vcam, new Vector3(0,0,0), Quaternion.identity);
            playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = camLookAt;
            playerCamera.GetComponent<CinemachineVirtualCamera>().LookAt = camLookAt;
        }
    }
}
