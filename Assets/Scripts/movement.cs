using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class movement : NetworkBehaviour
{
    public float moveSpeed = 3;
    public Animator a;
    private int hittedState;
    [HideInInspector] public Vector3 dir;
    public float hInput;
    public float vInput;
    //public bool isdead = false;
    
    NetworkCharacterControllerPrototypeCustom controller;
    
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        hittedState = Animator.StringToHash("Base Layer.GetHit01_SwordAndShield");
        //a = GameObject.Find("MaleCharacterPolyart").GetComponent<Animator>();
    }

    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
    }

    public override void FixedUpdateNetwork() {

        //if (isdead)
        //    return;


        if (GetInput(out NetworkInputData data)){
            dir = transform.forward * data.vInput * moveSpeed + transform.right * data.hInput * moveSpeed;
            controller.Move(dir * Runner.DeltaTime);
            if(data.isJump)
            {
                a.SetBool( "jump", true );
                controller.Jump();
            }

            else
            {
                a.SetBool( "jump", false );
                if ( (data.vInput != 0) || (data.hInput != 0) )
                {
                    a.SetBool( "run", true );
                }
                else
                {
                    a.SetBool( "run", false );
                }
            }
                
        }

        AnimatorStateInfo currentState = a.GetCurrentAnimatorStateInfo(0);
        if (currentState.fullPathHash == hittedState){
            a.SetBool( "hit", false );
        }
    }

    public Vector3 GetMovementVector()
    {
        return new Vector2(hInput, vInput);
    }

    public void teleport(Vector3 position)
    {
        controller.TeleportToPosition(position);
    }
    
}
