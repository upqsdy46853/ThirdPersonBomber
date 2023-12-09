using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class bullet : NetworkBehaviour
{
    [Networked] private TickTimer life {get;set;}
    public void Init(Vector3 startingVelocity){
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
        GetComponent<Rigidbody>().velocity = startingVelocity;
    }

    public override void FixedUpdateNetwork() {
        if(life.Expired(Runner))
            Runner.Despawn(Object);
    }
}