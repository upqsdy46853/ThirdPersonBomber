using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Amethyst : NetworkBehaviour
{
    [Networked] private TickTimer spawn_time { get; set; }
    bool isTrigger = false;

    public override void FixedUpdateNetwork()
    {
        if (isTrigger)
        {
            Runner.Despawn(Object);
        }
    }

    public void Init(Vector3 spawnDirection)
    {
        spawn_time = TickTimer.CreateFromSeconds(Runner, 2.0f);
        GetComponent<Rigidbody>().velocity = spawnDirection * 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority)
            return;

        if (!spawn_time.ExpiredOrNotRunning(Runner))
            return;

        if (other.gameObject.TryGetComponent<PlayerState>(out var state))
        {
            state.OnGetAmethyst();
            isTrigger = true;
        }

    }

}