using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class bullet : NetworkBehaviour
{
    [Header("Prefabs")]
    public GameObject explosionParticleSystemPrefab;
    // bomb control ========================
    public GameObject smokeParticleSystemPrefab;
    public GameObject blackParticleSystemPrefab;

    [Networked(OnChanged = nameof(OnBombIDChanged))]
    // bomb type, 0->default bomb, 1->smoke bomb, 2, black bomb
    public int bomb_id { get; set; }
    // =====================================
    [Networked] private TickTimer life {get;set;}
    [Networked] private TickTimer safe {get;set;}

    List<LagCompensatedHit> hits = new List<LagCompensatedHit>();
    public LayerMask collisionLayers;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    bool isHit = false;

    public void Init(Vector3 startingVelocity, int bomb_type){
        bomb_id = bomb_type;
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
        if (bomb_id == 0){
            int hitCount = Runner.LagCompensation.OverlapSphere(transform.position, 3f, Object.InputAuthority, hits, playerLayer, HitOptions.None);
            for(int i = 0; i<hitCount; i++){
                var obj = hits[i].Hitbox.Root;
                if(obj.TryGetComponent<PlayerState>(out var state)){
                    Vector3 direction = obj.transform.position - transform.position;
                    float distance = direction.magnitude;
                    bool blocked = Physics.Raycast(transform.position, direction.normalized, distance, groundLayer);
                    if(!blocked)
                    {
                        state.OnTakeDamage();
                    }
                }
            }
        }
        else if (bomb_id == 1){

        }
        else if (bomb_id == 2){
            int hitCount = Runner.LagCompensation.OverlapSphere(transform.position, 3f, Object.InputAuthority, hits, playerLayer, HitOptions.None);
            for(int i = 0; i<hitCount; i++){
                var obj = hits[i].Hitbox.Root;
                if(obj.TryGetComponent<PlayerState>(out var state)){
                    Vector3 direction = obj.transform.position - transform.position;
                    float distance = direction.magnitude;
                    bool blocked = Physics.Raycast(transform.position, direction.normalized, distance, groundLayer);
                    if(!blocked)
                    {
                        //state.OnBlackScreen();
                    }
                }
            }
        }
        Runner.Despawn(Object);
    }

    //When despawning the object we want to create a visual explosion
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        MeshRenderer grenadeMesh = GetComponentInChildren<MeshRenderer>();

        if(bomb_id == 0){
            GameObject particle = Instantiate(explosionParticleSystemPrefab, grenadeMesh.transform.position, Quaternion.identity);
            Destroy(particle, 5.0f);
        }
        else if(bomb_id == 1){
            GameObject particle = Instantiate(smokeParticleSystemPrefab, grenadeMesh.transform.position, Quaternion.identity);
            Destroy(particle, 15.0f);
        }
        else if(bomb_id == 2){
            GameObject particle = Instantiate(blackParticleSystemPrefab, grenadeMesh.transform.position, Quaternion.identity);
            Destroy(particle, 5.0f);
        }
    }

    static void OnBombIDChanged(Changed<bullet> changed)
    {}
}