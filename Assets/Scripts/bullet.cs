using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class bullet : NetworkBehaviour
{
    [Networked] private TickTimer life {get;set;}
    [Networked] private TickTimer safe {get;set;}

    List<LagCompensatedHit> hits = new List<LagCompensatedHit>();
    public LayerMask collisionLayers;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    bool isHit = false;

    public void Init(Vector3 startingVelocity){
        life = TickTimer.CreateFromSeconds(Runner, 10.0f);
        safe = TickTimer.CreateFromSeconds(Runner, 0.1f);

        StartCoroutine(spawnedCO(startingVelocity));
        //GetComponent<Rigidbody>().velocity = startingVelocity;
    }

    public override void FixedUpdateNetwork() {
        if(life.Expired(Runner))
            Runner.Despawn(Object);
        if(safe.Expired(Runner) && !isHit){
            int hitCount = Runner.LagCompensation.OverlapSphere(transform.position, 0.5f, Object.InputAuthority, hits, collisionLayers, HitOptions.IncludePhysX);
            if (hitCount > 0){
                StartCoroutine(explodeCO());
            }
        }
    }

    IEnumerator spawnedCO(Vector3  startingVelocity)
    {
        yield return new WaitForSeconds(0.01f);
        GetComponent<Rigidbody>().velocity = startingVelocity;
    }

    IEnumerator explodeCO(){
        isHit = true;
        yield return new WaitForSeconds(1f);
        int hitCount = Runner.LagCompensation.OverlapSphere(transform.position, 30f, Object.InputAuthority, hits, playerLayer, HitOptions.None);
        for(int i = 0; i<hitCount; i++){
            var obj = hits[i].Hitbox.Root;
            if(obj.TryGetComponent<PlayerState>(out var state)){
                Vector3 direction = obj.transform.position - transform.position;
                float distance = direction.magnitude;
                bool blocked = Physics.Raycast(transform.position, direction.normalized, distance, groundLayer);
                if(!blocked)
                    state.OnTakeDamage();
            }
        }
        Runner.Despawn(Object);
    }
}