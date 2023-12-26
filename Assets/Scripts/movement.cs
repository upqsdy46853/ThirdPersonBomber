using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class movement : NetworkBehaviour
{
    public float moveSpeed = 3;
    public Animator a_m;
    public Animator a_f;
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
        a = a_f;
        //a = GameObject.Find("MaleCharacterPolyart").GetComponent<Animator>();
    }

    void Update()
    {
        if(GetComponent<PlayerState>().HP > 0){
            hInput = Input.GetAxis("Horizontal");
            vInput = Input.GetAxis("Vertical");
        }
        else{
            hInput = 0;
            vInput = 0;
        }
        if ( gameObject.GetComponent<PlayerState>().Team == Color.red )
        {
            gameObject.transform.Find("MaleCharacterPolyart").gameObject.SetActive(true);
            gameObject.transform.Find("FemaleCharacterPolyart").gameObject.SetActive(false);
            a = a_m;
        }

        else
        {
            gameObject.transform.Find("MaleCharacterPolyart").gameObject.SetActive(false);
            gameObject.transform.Find("FemaleCharacterPolyart").gameObject.SetActive(true);
            a = a_f;
        }
    }

    public override void FixedUpdateNetwork() {

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
