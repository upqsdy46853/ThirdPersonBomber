using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class AmethystSpawner : SimulationBehaviour
{
    // Start is called before the first frame update
    public GameObject AmethystPrefab;
    public GameObject smokeItemPrefab;
    public GameObject blackItemPrefab;
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
            // spawning item
            float x1 = Random.Range(-10.0f, 10.0f);
            float z1 = Random.Range(-10.0f, 10.0f);
            float prob = Random.Range(0.0f, 1.0f);
            switch(prob){
                case <= 0.2f:
                    Runner.Spawn(
                        smokeItemPrefab, 
                        new Vector3(x1, 0.3f, z1), 
                        Quaternion.identity 
                    );
                    break;
                case <= 0.4f:
                    Runner.Spawn(
                        blackItemPrefab, 
                        new Vector3(x1, 0.3f, z1), 
                        Quaternion.identity 
                    );
                    break;
                default:
                    break;
            }
        }
    }
}
