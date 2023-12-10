using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class movement : NetworkBehaviour
{
    public float moveSpeed = 3;
    [HideInInspector] public Vector3 dir;
    public float hInput;
    public float vInput;

    NetworkCharacterControllerPrototypeCustom controller;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<NetworkCharacterControllerPrototypeCustom>();
    }

    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
    }

    public override void FixedUpdateNetwork() {
        if (GetInput(out NetworkInputData data)){
            dir = transform.forward * data.vInput * moveSpeed + transform.right * data.hInput * moveSpeed;
            controller.Move(dir * Runner.DeltaTime);
            if(data.isJump)
                controller.Jump();
        }
    }

    public Vector3 GetMovementVector()
    {
        return new Vector2(hInput, vInput);
    }


    
}
