using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;

public class movement : NetworkBehaviour
{
    public float moveSpeed = 3;
    public float jumpSpeed = 1f;
    [HideInInspector] public Vector3 dir;
    float hInput, vInput;
    CharacterController controller;
    public GameObject CM_vcam;
    private bool _jumpPressed;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false){
            return;
        }
        if(controller.isGrounded){
            hInput = Input.GetAxis("Horizontal");
            vInput = Input.GetAxis("Vertical");
            dir = transform.forward * vInput * moveSpeed + transform.right * hInput * moveSpeed;
            if(_jumpPressed){
                dir.y += jumpSpeed;
            }
        }
        dir += Physics.gravity * Runner.DeltaTime;
        controller.Move(dir * Runner.DeltaTime);
        _jumpPressed = false;
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            GameObject playerCamera = Instantiate(CM_vcam, new Vector3(0,0,0), Quaternion.identity);
            var body = transform.Find("Body");
            var camLookAt = body.Find("camLookAt");
        
            playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = camLookAt;
            playerCamera.GetComponent<CinemachineVirtualCamera>().LookAt = camLookAt;

        }
    }
}
