using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Fusion;

public class camControl : NetworkBehaviour
{
    // Start is called before the first frame update
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camLookAt;
    public GameObject CM_vcam;
    

    void Awake(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    public override void FixedUpdateNetwork() {
        if (GetInput(out NetworkInputData data)){
            camLookAt.localEulerAngles = new Vector3(data.yAxis, camLookAt.localEulerAngles.y, camLookAt.localEulerAngles.z); 
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, data.xAxis, transform.eulerAngles.z);
        }
    }

    public override void Spawned(){
        if (HasInputAuthority){
            GameObject playerCamera = Instantiate(CM_vcam, new Vector3(0,0,0), Quaternion.identity);
            playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = camLookAt;
            playerCamera.GetComponent<CinemachineVirtualCamera>().LookAt = camLookAt;
        }
    }
    
    public Vector2 GetViewVector(){
        return new Vector2(xAxis.Value, yAxis.Value);
    }


}
