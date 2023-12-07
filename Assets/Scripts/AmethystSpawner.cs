using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class AmethystSpawner : SimulationBehaviour
{
    public GameObject AmethystPrefab;
    private TickTimer _powerupTimer;

    public override void FixedUpdateNetwork(){
        if (_powerupTimer.ExpiredOrNotRunning(Runner)){
            _powerupTimer = TickTimer.CreateFromSeconds(Runner, 5);

            float x = Random.Range(-10.0f, 10.0f);
            float z = Random.Range(-10.0f, 10.0f);
            Runner.Spawn(
                AmethystPrefab, 
                new Vector3(x, 0.3f, z), 
                Quaternion.identity 
            );
        }

    }
}
