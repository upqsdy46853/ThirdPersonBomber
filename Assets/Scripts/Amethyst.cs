using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Amethyst : NetworkBehaviour
{
    bool isTrigger = false;

    public override void FixedUpdateNetwork() {
        if(isTrigger){
            Runner.Despawn(Object);
        }
    }


    private void OnTriggerEnter(Collider other){
        if(!HasStateAuthority)
            return;
        //print(other.gameObject.name);

        if (other.gameObject.TryGetComponent<PlayerState>(out var player)){
            player.DealAmethystRpc();
            isTrigger = true;
        }
            
    }

}
