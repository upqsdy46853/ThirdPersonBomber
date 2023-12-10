using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ReadyUIHandler : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        Runner.SessionInfo.IsOpen = false;
        GameObject[] gameObjectsToTransfer = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject gameObjectToTransfer in gameObjectsToTransfer)
        {
            DontDestroyOnLoad(gameObjectToTransfer);
        }
        Runner.SetActiveScene("Game");
    }
}
