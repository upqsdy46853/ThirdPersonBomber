using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class movement : NetworkBehaviour
{
    public float moveSpeed = 3;
    public float jumpSpeed = 1f;
    [HideInInspector] public Vector3 dir;
    float hInput, vInput;
    CharacterController controller;
    private bool _jumpPressed;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
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
            dir = transform.forward * vInput * moveSpeed + transform.right * hInput * moveSpeed;
            if(_jumpPressed){
                dir.y += jumpSpeed;
            }
        }
        dir += Physics.gravity * Runner.DeltaTime;
        controller.Move(dir * Runner.DeltaTime);
        _jumpPressed = false;
    }

}
